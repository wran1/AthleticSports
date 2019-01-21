using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

using Common;
using Models.SysModels;
using IServices.ISysServices;
using IServices.Infrastructure;
using IServices;
using Services.SysServices;
using Web.Providers;
using System.Configuration;
using System.Web.Razor.Generator;
using Web.Areas.Api.Models;
using Web.Helpers;
using Services;
using Web.SignalR;
using System.Net;
using System.IO;
using System.Text;

namespace Web.Areas.Api.Controllers
{
    /// <summary>
    /// 账户操作
    /// </summary>
    [Authorize]
    [RoutePrefix("API/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private readonly DateTime _miniDataTime = new DateTime(2016,1,1,0,0,0,0);
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        private readonly IUserInfo _iUserInfo;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly ISysUserService _iSysUserService;
        private readonly EmailService _emailService;
        public AccountController(ISysUserService iSysUserService,  IUnitOfWork iUnitOfWork, IUserInfo iUserInfo)
        {
            _iSysUserService = iSysUserService;
            _iUnitOfWork = iUnitOfWork;
            _iUserInfo = iUserInfo;
            _emailService = new EmailService();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        /// <summary>
        /// 获取手机号注册状态
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>true为已注册，false为未注册</returns>
        [AllowAnonymous,Route("PhoneResiterStatus")]
        public APIResult<bool> GetCheckPhoneExists(string phoneNumber)
        {
            var user = _iSysUserService.GetAll().FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            if (user == null)
                return new APIResult<bool>(false);
            return new APIResult<bool>(true);
        }
        /// <summary>
        /// 获取用户名是否已被占用
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>True为已占用</returns>
        [AllowAnonymous, Route("UserNameExists")]
        public APIResult<bool> GetUserNameExists(string userName)
        {
            var currentUserName = _iUserInfo.UserName;
            if (_iSysUserService.GetAll().Any(u => u.UserName == userName && u.UserName != currentUserName))
            {
                return new APIResult<bool>(true);
            }
            return new APIResult<bool>(false);
        }
        ///// <summary>
        ///// 手机注册
        ///// </summary>W
        ///// <param name="registerBindModel"></param>
        ///// <returns></returns>
        //[Route("Register")]
        //[AllowAnonymous]
        //public async Task<APIResult<AccessTokenViewModel>> Register(RegisterBindModel registerBindModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result4CodeVerify =
        //            await
        //                PhoneVerifyCodeProvider.CodeVerify(registerBindModel.PhoneNumber, registerBindModel.VerifyCode,
        //                    TimeSpan.Zero);
        //        if (result4CodeVerify.Success)
        //        {
        //            // 创建新用户并返回AccessToken
        //            var user =
        //                _iSysUserService.GetAll()
        //                    .FirstOrDefault(u => u.PhoneNumber == registerBindModel.PhoneNumber);
        //            if (user == null)
        //            {
        //                var sjs = Guid.NewGuid().ToString().Substring(0, 4);
        //                user = new SysUser
        //                {

        //                    Id = Guid.NewGuid().ToString(),
        //                    UserName = CommonCodeGenerator.GenerateUserName(sjs + registerBindModel.PhoneNumber),//"yw_" + (DateTimeLocal.Now-_miniDataTime).TotalDays.ToString("f0").PadLeft(6,'0')+registerBindModel.PhoneNumber,
        //                    PhoneNumber = registerBindModel.PhoneNumber,
        //                    PhoneNumberConfirmed = true,
        //                    TwoFactorEnabled = false,//不自动开启二次验证
        //                    LockoutEnabled = true,
        //                };
        //                user.Email = CommonCodeGenerator.GenerateEmail(user.UserName);
        //                IdentityResult result = await UserManager.CreateAsync(user, registerBindModel.Password);
        //                if (result.Succeeded)
        //                {
        //                    result = await UserManager.AddToRoleAsync(user.Id, "注册用户");
        //                }
        //                if (!result.Succeeded)
        //                {
        //                    SetModelState(result);
        //                }


        //            }
        //            else
        //            {
        //                ModelState.AddModelError("PhoneNumber", "该手机账户已经存在");
        //            }
        //            if (ModelState.IsValid)
        //            {
        //                var data = await GetAccessToken(user); //登录
        //                if (data != null)
        //                    return new APIResult<AccessTokenViewModel>(data, 0, "注册并登录成功");
        //            }
        //            //else
        //            //{
        //            //    return new APIResult<AccessTokenViewModel>(null, 100, "注册失败", ModelState);
        //            //}
        //        }
        //        else
        //        {
        //            foreach (var err in result4CodeVerify.ErroMessage)
        //            {
        //                ModelState.AddModelError(err.Key, err.Value);
        //            }
        //        }
        //    }
        //    return new APIResult<AccessTokenViewModel>(null, 100, "注册失败", ModelState);//Todo:可能是注册成功，登录失败
        //}
        /// <summary>
        /// 通过密码登录
        /// </summary>
        /// <param name="loginBindModel"></param>
        /// <returns></returns>
        [Route("Login")]
        [AllowAnonymous]
        public async Task<APIResult<AccessTokenViewModel>> Login(LoginBindModel loginBindModel)
        {
            if (ModelState.IsValid) { 
                var user =
                    _iSysUserService.GetAll()
                        .FirstOrDefault(
                            u =>
                                u.UserName == loginBindModel.UserName || u.Email == loginBindModel.UserName ||
                                u.PhoneNumber == loginBindModel.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("UserName", "用户不存在");
                }
                else { 
                    var result = await SignInManager.PasswordSignInAsync(user.UserName, loginBindModel.Password, false, true);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            var data = await GetAccessToken(user);
                            if (data != null) { 
                                return new APIResult<AccessTokenViewModel>(data,0,"登录成功");
                            }
                            break;
                        case SignInStatus.RequiresVerification://需要验证，转到验证步骤
                            return new APIResult<AccessTokenViewModel>(null, 1, "需要验证后方能登录");
                        case SignInStatus.LockedOut:
                            ModelState.AddModelError("", "用户已锁定");
                            break;
                        default:
                            ModelState.AddModelError("Password", "密码错误"); 
                            break; 
                    }
                }
            }
            return new APIResult<AccessTokenViewModel>(null,100, "登录失败",ModelState);
        }
        

        ///// <summary>
        ///// 手机验证登录，如果没有该手机用户将自动注册 【开发完成】【最后发布日期 2016-10-29】
        ///// </summary>
        ///// <param name="loginByPhoneBindModel"></param>
        ///// <returns></returns>
        //[Route("LoginByPhone")] 
        //[AllowAnonymous]
        //public async Task<APIResult<AccessTokenViewModel>> LoginByPhone(LoginByPhoneBindModel loginByPhoneBindModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result4CodeVerify =
        //            await
        //                PhoneVerifyCodeProvider.CodeVerify(loginByPhoneBindModel.PhoneNumber,
        //                    loginByPhoneBindModel.VerifyCode, TimeSpan.Zero);
        //        if (result4CodeVerify.Success)
        //        {
        //            var user =
        //                _iSysUserService.GetAll()
        //                    .FirstOrDefault(u => u.PhoneNumber == loginByPhoneBindModel.PhoneNumber);
        //            if (user == null)
        //            {
        //                //创建新用户
        //                user = new SysUser
        //                {
        //                    Id = Guid.NewGuid().ToString(),
        //                    UserName = CommonCodeGenerator.GenerateUserName((DateTimeLocal.Now - _miniDataTime).TotalDays.ToString("f0").PadLeft(6, '0') + loginByPhoneBindModel.PhoneNumber),// "ZhiWei" + (DateTimeLocal.Now - _miniDataTime).TotalDays.ToString("f0").PadLeft(6, '0') + loginByPhoneBindModel.PhoneNumber,
        //                    PhoneNumber = loginByPhoneBindModel.PhoneNumber,
        //                    PhoneNumberConfirmed = true,
        //                    TwoFactorEnabled = false,
        //                    LockoutEnabled = true,
        //                 };
        //                user.Email = CommonCodeGenerator.GenerateEmail(user.UserName);// user.UserName + "@wjw1.com";
        //                IdentityResult result = await UserManager.CreateAsync(user);//,"morenmima");
        //                if (!result.Succeeded)
        //                {
        //                    SetModelState(result);
        //                }
        //                result = await UserManager.AddToRoleAsync(user.Id, "注册用户");
        //                if (!result.Succeeded)
        //                {
        //                    SetModelState(result);
        //                }
                        
                       
        //            }
        //            if(ModelState.IsValid)
        //            { 
        //                var data= await GetAccessToken(user);//登录
        //                if (data != null)
        //                    return new APIResult<AccessTokenViewModel>(data, 0, "登录成功");
        //            }
        //        }
        //        else
        //        {
        //            foreach (var err in result4CodeVerify.ErroMessage)
        //            {
        //                ModelState.AddModelError(err.Key,err.Value);
        //            }
        //        }
        //    }
        //    return new APIResult<AccessTokenViewModel>(null, 100, "登录失败", ModelState);
        //}
        
        /// <summary>
        /// 注销 需客户端手动清除AccessToken
        /// </summary>
        /// <returns></returns>
        [Route("Logout")]
        public async Task<APIResult<bool>> Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            Authentication.SignOut(OAuthDefaults.AuthenticationType);
            return await Task.FromResult<APIResult<bool>>(new APIResult<bool>(true));
        }

        /// <summary>
        /// 刷新 AccessToken
        /// </summary>
        /// <returns></returns>
        [Route("RefreshAccessToken")]
        public async Task<APIResult<AccessTokenViewModel>> RefreshAccessToken()
        {
            var user = _iSysUserService.GetAll().FirstOrDefault(u => u.UserName == User.Identity.Name);
            var data = await GetAccessToken(user);
            if (data == null)
            {
                return new APIResult<AccessTokenViewModel>(null, 100, "获取新的AccessToken失败", ModelState);
            }
            return new APIResult<AccessTokenViewModel>(data);
        }
      
        ///// <summary>
        ///// 设置手机号，登录状态下可操作，如果是修改手机号，客户端需添加验证步骤（验证绑定的邮箱或手机号）
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[Route("SetPhone")]
        //public async Task<APIResult<bool>> SetPhone(VerifyCode4PhoneBindModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userId = _iUserInfo.UserId;
        //        if (_iSysUserService.GetAll().Any(u => u.PhoneNumber == model.PhoneNumber && u.Id != userId))
        //        {
        //            ModelState.AddModelError("PhoneNumber", "该手机号已绑定到其他账户");
        //        }
        //        else
        //        {
        //            var user =
        //                _iSysUserService.GetAll()
        //                    .FirstOrDefault(u => u.Id == userId);
        //            if (user != null)
        //            {
        //                if (user.PhoneNumber == model.PhoneNumber)
        //                {
        //                    ModelState.AddModelError("PhoneNumber", "设置的手机号为原来绑定的手机号");
        //                }
        //                else
        //                {
        //                    var result4CodeVerify =
        //                        await
        //                            PhoneVerifyCodeProvider.CodeVerify(model.PhoneNumber, model.VerifyCode,
        //                                TimeSpan.Zero);
        //                    if (result4CodeVerify.Success)
        //                    {
        //                        user.PhoneNumber = model.PhoneNumber;
        //                        user.PhoneNumberConfirmed = true;
        //                        _iSysUserService.Save(user.Id, user);
        //                        await _iUnitOfWork.CommitAsync();

                               

        //                        return new APIResult<bool>(true);
        //                    }
        //                    else
        //                    {
        //                        foreach (var err in result4CodeVerify.ErroMessage)
        //                        {
        //                            ModelState.AddModelError(err.Key,err.Value);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return new APIResult<bool>(false, 100, "设置手机号失败", ModelState);
        //}

        ///// <summary>
        ///// 设置邮箱，登录状态下可操作，如果是修改邮箱，客户端需添加验证步骤（验证绑定的邮箱或手机号）
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[Route("SetEmail")]
        //public async Task<APIResult<bool>> SetEmail(VerifyCode4EmailBindModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userId = _iUserInfo.UserId;
        //        if (_iSysUserService.GetAll().Any(u => u.Email == model.Email && u.Id != userId))
        //        {
        //            ModelState.AddModelError("Email", "该邮箱已绑定到其他账户");
        //        }
        //        else
        //        {
        //            var user =
        //                _iSysUserService.GetAll()
        //                    .FirstOrDefault(u => u.Id == userId);
        //            if (user != null)
        //            {
        //                if (user.Email == model.Email)
        //                {
        //                    ModelState.AddModelError("Email", "设置的邮箱为原来绑定的邮箱");
        //                }
        //                else
        //                {
        //                    var result4CodeVerify = await EmailVerifyCodeProvider.CodeVerify(model.Email, model.VerifyCode,TimeSpan.Zero);
        //                    if (result4CodeVerify.Success)
        //                    {
        //                        user.Email = model.Email;
        //                        user.EmailConfirmed = true;
        //                        _iSysUserService.Save(user.Id, user);
        //                        //奖励积分
                               
        //                        await _iUnitOfWork.CommitAsync();
        //                        return new APIResult<bool>(true);
        //                    }
        //                    else
        //                    {
        //                        foreach (var err in result4CodeVerify.ErroMessage)
        //                        {
        //                            ModelState.AddModelError(err.Key,err.Value);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return new APIResult<bool>(false, 100, "设置邮箱失败", ModelState);
        //}

        /// <summary>
        /// 解绑手机，需在登录状态下，且客户端在该操作前完成手机或邮箱验证，如果手机是唯一绑定的项不支持解除绑定操作
        /// </summary>
        /// <returns></returns>
        [Route("UnBindPhone")]
        public async Task<APIResult<bool>> UnBindPhone()
        {
            var userId = _iUserInfo.UserId;
            var user = _iSysUserService.GetById(userId);
            if (!user.EmailConfirmed) //手机是否为唯一绑定的项
            {
                ModelState.AddModelError("", "手机是您当前唯一设置的绑定，不能解除绑定");
            }
            else
            {
                user.PhoneNumber = null;
                user.PhoneNumberConfirmed = false;
                _iSysUserService.Save(userId,user);
                await _iUnitOfWork.CommitAsync();
                return new APIResult<bool>(true);
            }
            return new APIResult<bool>(false, 100, "操作失败", ModelState);
        }


        
        ///// <summary>
        ///// 设置密码  发布时间[2017/5/5]
        ///// </summary>
        ///// <param name="registerBindModel"></param>
        ///// <returns></returns>
        //[Route("SetPassword")]
        //public async Task<APIResult<bool>> SetPassword(RegisterBindModel registerBindModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result4CodeVerify =
        //            await
        //                PhoneVerifyCodeProvider.CodeVerify(registerBindModel.PhoneNumber, registerBindModel.VerifyCode,
        //                    TimeSpan.Zero);
        //        if (result4CodeVerify.Success)
        //        {
        //            var user =
        //             _iSysUserService.GetAll()
        //                 .FirstOrDefault(u => u.PhoneNumber == registerBindModel.PhoneNumber);
        //            var result = await
        //                UserManager.AddPasswordAsync(_iUserInfo.UserId, registerBindModel.Password);
        //            if (result.Succeeded)
        //            {
        //                return new APIResult<bool>(true);
        //            }
        //            else
        //            {
        //                SetModelState(result);
        //            }
        //        }

        //    }
        //    return new APIResult<bool>(false, 100, "设置密码失败", ModelState);
        //}
        /// <summary>
        /// 登录状态下更改密码 【开发完成】【最后发布日期：2016-10-21】
        /// </summary>
        /// <param name="passwordBindModel"></param>
        /// <returns></returns>
        [Route("ChangePassword")]
        public async Task<APIResult<bool>> ChangePassword(PasswordBindModel passwordBindModel)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await
                        UserManager.ChangePasswordAsync(_iUserInfo.UserId, passwordBindModel.CurrentPassword,
                            passwordBindModel.Password);
                if (result.Succeeded)
                {
                    return new APIResult<bool>(true);
                }
                else
                {
                    SetModelState(result);
                }
           
            }
            return new APIResult<bool>(false, 100, "修改密码失败", ModelState);
        }
        /// <summary>
        /// 通过手机验证重置密码 【开发完成】【最后发布日期 2016-10-29】
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous,Route("ReSetPasswordByPhoneCode")]
        public async Task<APIResult<bool>> ReSetPasswordByPhoneCode(
            ReSetPasswordByPhoneCodeBindModel model)
        {
            if (ModelState.IsValid)
            {
                var user =
                    _iSysUserService.GetAll()
                        .FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);
                if (user != null)
                {
                    await UserManager.RemovePasswordAsync(user.Id);
                    var result = await UserManager.AddPasswordAsync(user.Id, model.Password);
                    if (result.Succeeded)
                    {
                        return new APIResult<bool>(true);
                    }
                    SetModelState(result);
                }
                else
                {
                    ModelState.AddModelError("PhoneNumber", "未找该手机对应的账户");
                }
            }
            return new APIResult<bool>(false, 100, "密码重置失败", ModelState);
        }


        /// <summary>
        /// 通过邮箱验证重置密码 【开发完成】【最后发布日期 2016-10-29】
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous, Route("ReSetPasswordByEmailCode")]
        public async Task<APIResult<bool>> ReSetPasswordByEmailCode(
            ReSetPasswordByEmailCodeBindModel model)
        {
            if (ModelState.IsValid)
            {
                var user =
                    _iSysUserService.GetAll()
                        .FirstOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    await UserManager.RemovePasswordAsync(user.Id);//已设置过密码的用户需移除密码
                    var result = await UserManager.AddPasswordAsync(user.Id, model.Password);
                    if (result.Succeeded)
                    {
                        return new APIResult<bool>(true);
                    }
                    SetModelState(result);
                }
                else
                {
                    ModelState.AddModelError("Email", "未找到邮箱对应的账户");
                }
            }
            return new APIResult<bool>(false, 100, "密码重置失败", ModelState);
        }
        

        //#region 验证码
        ///// <summary>
        ///// 发送手机验证码
        ///// </summary>
        ///// <param name="model">SendCode2PhoneBindModel</param>
        ///// <returns>IHttpActionResult</returns>
        //[AllowAnonymous]
        //[Route("SendCode2Phone")]
        //public async Task<APIResult<bool>> SendCode2Phone(SendCode2PhoneBindModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var code = CommonCodeGenerator.Generator(6);
        //        var phone = model.PhoneNumber;
        //        var result4SendCode = await PhoneVerifyCodeProvider.SendCode(code, phone, "您的验证码为 ");
        //        if (result4SendCode.Success)
        //        {
        //            return await Task.FromResult(new APIResult<bool>(true));
        //        }
        //        foreach (var item in result4SendCode.ErroMessage)
        //        {
        //            ModelState.AddModelError(item.Key, item.Value);
        //        }
        //    }
        //    return new APIResult<bool>(false,100,"获取手机验证码失败",ModelState);
        //}

        ///// <summary>
        ///// 发送邮箱验证码,更换邮箱时使用
        ///// </summary>
        ///// <param name="model">SendCode2EmailBindModel</param>
        ///// <returns></returns>
        //[AllowAnonymous]
        //[Route("SendCode2NewEmail")]
        //public async Task<APIResult<bool>> SendCode2NewEmail(SendCode2EmailBindModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (_iSysUserService.GetAll().Any(u => u.Email == model.Email))
        //        {
        //            ModelState.AddModelError("Email", "已存在使用该邮箱的账户");
        //        }
        //        var code = CommonCodeGenerator.Generator(6);
        //        var email = model.Email;
        //        var result4SendCode = await EmailVerifyCodeProvider.SendCode(code, email, "您的验证码为 ");
        //        if (result4SendCode.Success)
        //        {
        //            return await Task.FromResult(new APIResult<bool>(true));
        //        }
        //        foreach (var item in result4SendCode.ErroMessage)
        //        {
        //            ModelState.AddModelError(item.Key, item.Value);
        //        }
        //    }
        //    return await Task.FromResult(new APIResult<bool>(false, 100, "获取邮箱验证码失败", ModelState));
        //}


        ///// <summary>
        ///// 发送邮箱验证码,验证操作使用
        ///// </summary>
        ///// <param name="model">SendCode2EmailBindModel</param>
        ///// <returns></returns>
        //[AllowAnonymous]
        //[Route("SendCode2Email4VerifyOp")]
        //public async Task<APIResult<bool>> SendCode2Email4VerifyOp(SendCode2EmailBindModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (!_iSysUserService.GetAll().Any(u => u.Email == model.Email))
        //        {
        //            ModelState.AddModelError("Email", "未找到使用该邮箱的账户，请输入您绑定的邮箱");
        //        }
        //        var code = CommonCodeGenerator.Generator(6);
        //        var email = model.Email;
        //        //调用统一验证码程序
        //        var result4SendCode  = await EmailVerifyCodeProvider.SendCode(code,email, "您的验证码为 ");
        //        if (result4SendCode.Success)
        //        {
        //            return await Task.FromResult(new APIResult<bool>(true));
        //        }
        //        foreach (var item in result4SendCode.ErroMessage)
        //        {
        //            ModelState.AddModelError(item.Key,item.Value);
        //        }
        //    }
        //    return await Task.FromResult(new APIResult<bool>(false, 100, "获取邮箱验证码失败", ModelState));
        //}

        ///// <summary>
        ///// 验证新手机号并发送手机验证码，注册和更换新手机号时使用，如果已存在该手机号的用户，提示已存在该手机号的用户
        ///// </summary>
        ///// <param name="model">SendCode2PhoneBindModel</param>
        ///// <returns>IHttpActionResult</returns>
        //[AllowAnonymous]
        //[Route("SendCode2NewPhone4")]
        //public async Task<APIResult<bool>> SendCode2NewPhone(SendCode2PhoneBindModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (_iSysUserService.GetAll().Any(u=>u.PhoneNumber== model.PhoneNumber))
        //        {
        //            ModelState.AddModelError("PhoneNumber", "已存在使用该手机号的账户，一个手机号只能绑定一个账号");
        //        }
        //        else {
        //            var code = CommonCodeGenerator.Generator(6);
        //            var phone = model.PhoneNumber;
        //            var result4SendCode = await PhoneVerifyCodeProvider.SendCode(code, phone, "您的验证码为 ");
        //            if (result4SendCode.Success)
        //            {
        //                return new APIResult<bool>(true, 0, "验证码已发送");
        //            }
        //            foreach (var err in result4SendCode.ErroMessage)
        //            {
        //                ModelState.AddModelError(err.Key,err.Value);
        //            }
        //        }
        //    }
        //    return  new APIResult<bool>(false, 100, "验证码发送失败",ModelState); 
        //}

        ///// <summary>
        ///// 发送手机验证码,验证操作用，如果手机号用户不存在，提示“找不到使用该手机号的账户，请输入您绑定的手机号码”
        ///// </summary>
        ///// <param name="model">SendCode2PhoneBindModel</param>
        ///// <returns>IHttpActionResult</returns>
        //[AllowAnonymous]
        //[Route("SendCode2Phone4VerifyOp")]
        //public async Task<APIResult<bool>> SendCode2Phone4VerifyOp(SendCode2PhoneBindModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (!_iSysUserService.GetAll().Any(u => u.PhoneNumber == model.PhoneNumber))
        //        {
        //            ModelState.AddModelError("PhoneNumber", "找不到使用该手机号的账户，请输入您绑定的手机号码");
        //        }
        //        else {
        //            var verifyCode = CommonCodeGenerator.Generator(6);
        //            var verifyPhone = model.PhoneNumber;
        //            var result4Send = await PhoneVerifyCodeProvider.SendCode(verifyCode, verifyPhone, "您的验证码为 ");
        //            if (result4Send.Success)
        //            {
        //                return  new APIResult<bool>(true, 0, "验证码已发送"); ;
        //            }
        //            foreach (var err in result4Send.ErroMessage)
        //            {
        //                ModelState.AddModelError(err.Key,err.Value);
        //            }
        //        }
        //    }
        //    return new APIResult<bool>(false, 100, "验证码发送失败",ModelState);
        //}


        ///// <summary>
        ///// 验证手机验证码
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[AllowAnonymous]
        //[Route("VerifyCode4Phone")]
        //public async Task<APIResult<bool>>  VerifyCode4Phone(VerifyCode4PhoneBindModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return await Task.FromResult<APIResult<bool>>(new APIResult<bool>(false, 100, "数据验证错误", ModelState));
        //    }
        //    var result4CodeVerify =await PhoneVerifyCodeProvider.CodeVerify(model.PhoneNumber,model.VerifyCode,TimeSpan.FromMinutes(model.DelayMinutes));
        //    if (!result4CodeVerify.Success)
        //    {
        //        foreach (var err in result4CodeVerify.ErroMessage)
        //        {
        //            ModelState.AddModelError(err.Key,err.Value);
        //        }
        //        return await Task.FromResult<APIResult<bool>>(new APIResult<bool>(false, 100, "手机验证码错误或失效", ModelState));
        //    }
        //    return  await Task.FromResult<APIResult<bool>>(new APIResult<bool>(true,0,"手机验证码有效"));
        //}

        ///// <summary>
        ///// 验证邮箱验证码
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[AllowAnonymous]
        //[Route("VerifyCode4Email")]
        //public async Task<APIResult<bool>> VerifyCode4Email(VerifyCode4EmailBindModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return await Task.FromResult<APIResult<bool>>(new APIResult<bool>(false, 100, "数据验证错误", ModelState));
        //    }
        //    var result4CodeVerify = await EmailVerifyCodeProvider.CodeVerify(model.Email, model.VerifyCode, TimeSpan.FromMinutes(model.DelayMinutes));
        //    if (!result4CodeVerify.Success)
        //    {
        //        foreach (var err in result4CodeVerify.ErroMessage)
        //        {
        //            ModelState.AddModelError(err.Key, err.Value);
        //        }
        //        return await Task.FromResult<APIResult<bool>>(new APIResult<bool>(false, 100, "手机验证码错误或失效", ModelState));
        //    }
        //    return await Task.FromResult<APIResult<bool>>(new APIResult<bool>(true, 0, "邮箱验证码有效"));
        //}
        //#endregion

        
      /// <summary>
      /// 加密
      /// </summary>
      /// <param name="key">密钥</param>
      /// <param name="input">要加密的串</param>
      /// <returns></returns>
       [NonAction]
        public string HMACSHAsecret(string key,string input)
        {
            byte[] keyBytes = ASCIIEncoding.ASCII.GetBytes(key);
            byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(input);
            HMACSHA1 hmac = new HMACSHA1(keyBytes);
            byte[] hashBytes = hmac.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }
        ///// <summary>
        ///// 验证是否有账号信息  开发测试中
        ///// </summary>
        ///// <param name="unionid"></param>
        ///// <param name="openIdCatalog"></param>
        ///// <returns>0:成功、100:获取失败、200:还未关联</returns>
        //[AllowAnonymous]
        //[Route("YZAppRelated")]
        //public async Task<APIResult<AccessTokenViewModel>> YZAppRelated(string unionid,string openIdCatalog)
        //{
        //    var ob =
        //      _iOpenIdBindService.GetAll()
        //          .FirstOrDefault(a => a.OpenIdCatalog == openIdCatalog && a.OpenId == unionid);
        //    if (ob != null)
        //    {
        //        var usercreat = _iSysUserService.GetById(ob.Remark);
        //        var token = await GetAccessToken(usercreat);
        //        if (token != null)
        //        {
        //            return new APIResult<AccessTokenViewModel>(token);
        //        }
        //        return new APIResult<AccessTokenViewModel>(null, 100, "获取令牌失败，请稍后再试");
        //    }
        //    else
        //    {
        //        return new APIResult<AccessTokenViewModel>(null, 200, "还未关联应用平台账号");
        //    }
        //}
        //[AllowAnonymous]
        //[Route("QQYZ")]
        //public APIResult<string> QQYZ(string AccessToken)
        //{

        //}
      
        #region 帮助程序

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }
        

        private void SetModelState(IdentityResult result)
        {
            if (result != null && !result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error);
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定用户的AccessToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<AccessTokenViewModel> GetAccessToken(SysUser user)
        {
            if (user != null)
            {
                //if (user.LockoutEndDateUtc > DateTimeLocal.Now)
                //{
                //    ModelState.AddModelError("", "用户已锁定");
                //}
                //else { 
                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
                //生成ticket
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                var currentUtc = DateTime.Now;
                ticket.Properties.IssuedUtc = currentUtc;
                ticket.Properties.ExpiresUtc = currentUtc.Add(Startup.OAuthOptions.AccessTokenExpireTimeSpan);
                var token = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
                var expirers = (Int32)Startup.OAuthOptions.AccessTokenExpireTimeSpan.TotalSeconds;
                return new AccessTokenViewModel(token, "bearer", user.UserName, expirers - 1);
                //}
            }
            ModelState.AddModelError("", "无效的登录尝试");
            return null;
        }
        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }
                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits 必须能被 8 整除。", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }
        
        #endregion
    }
}