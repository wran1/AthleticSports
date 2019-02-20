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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Models.SysModels;
using IServices.ISysServices;
using IServices.Infrastructure;
using Services.SysServices;
using Web.Providers;
using Web.Areas.Api.Models;
using System.Text;
using IServices.IDictionaryServices;

namespace Web.Areas.Api.Controllers
{
    [Authorize]
    [RoutePrefix("API/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        private readonly IUserInfo _iUserInfo;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly ISysUserService _iSysUserService;
        private readonly EmailService _emailService;
        private readonly ISysDepartmentSysUserService _iSysDepartmentSysUserService;
        private readonly ISysDepartmentService _iDepartmentService;
        private readonly ITrainService _iTrainService;
        private readonly ISysRoleService _isysRoleService;
        public AccountController(ISysRoleService isysRoleService,ITrainService iTrainService,ISysUserService iSysUserService, IUnitOfWork iUnitOfWork, IUserInfo iUserInfo, ISysDepartmentService iDepartmentService, ISysDepartmentSysUserService iSysDepartmentSysUserService)
        {
            _iSysUserService = iSysUserService;
            _iUnitOfWork = iUnitOfWork;
            _iUserInfo = iUserInfo;
            _emailService = new EmailService();
            _iSysDepartmentSysUserService = iSysDepartmentSysUserService;
            _iDepartmentService = iDepartmentService;
            _iTrainService = iTrainService;
            _isysRoleService = isysRoleService;
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
        /// 获取所有教练(教练是两级，必须选择第二级)  
        /// 客户端数据处理方式：一级获取Systemcityid长度是3位，下一级的是上一级的Systemcityid+3位长度
        /// </summary>
        /// <returns>True为已占用</returns>
        [AllowAnonymous, Route("GetAllDepartment")]
        public APIResult<List<DepartmentModel>> GetAllDepartment()
        {
          var departList = _iDepartmentService.GetAll().Select(a=> new DepartmentModel {
               Id=a.Id,
               Name=a.Name
          }).ToList();
          return new APIResult<List<DepartmentModel>>(departList);
        }
        /// <summary>
        /// 获取所有专项 
        /// 客户端数据处理方式：一级获取Systemcityid长度是3位，下一级的是上一级的Systemcityid+3位长度
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>True为已占用</returns>
        [AllowAnonymous, Route("GetAllTrain")]
        public APIResult<List<TrainModel>> GetAllTrain(string userName)
        {
            var trainList = _iTrainService.GetAll().OrderBy(d => d.SystemId).Select(d=>new TrainModel {
                Id=d.Id,
                SystemId=d.SystemId,
                Name=d.Name
            }).ToList();
            return new APIResult<List<TrainModel>>(trainList);
        }
        /// <summary>
        /// 如果是教练或者总教练角色，获取所属运动员
        /// </summary>
        /// <returns></returns>
        [Route("GetMySportsman")]
         public APIResult<List<SportManModel>> GetMySportsman()
        {
            var DepartId = _iSysDepartmentSysUserService.GetAll(a => a.SysUserId == _iUserInfo.UserId).FirstOrDefault().SysDepartmentId;
            var roleId = _iSysUserService.GetById(_iUserInfo.UserId).Roles.FirstOrDefault().RoleId;
            var system=_iDepartmentService.GetById(DepartId).SystemId;
            var rolename = _isysRoleService.GetById(roleId).RoleName;
            var sportid = _isysRoleService.GetAll(a => a.RoleName == "运动员").FirstOrDefault().Id;
            var userlist = new List<SportManModel>();
            var result = new List<SportManModel>();
            if (rolename=="总教练" || (rolename == "教练" && system.Length == 6))
            {
                var systemlist = _iDepartmentService.GetAll(a => a.SystemId.StartsWith(system) && a.SystemId.Length == 6).Select(a=>a.Id).ToList();
              
                foreach (var item in systemlist)
                {
                    var userid = _iSysDepartmentSysUserService.GetAll(a => a.SysDepartmentId == item).Select(a => new SportManModel { UserId=a.SysUserId, FullName=a.SysUser.FullName } ).ToList();
                    userlist.AddRange(userid);
                }
                foreach (var item in userlist)
                {
                    if (_iSysUserService.GetById(item.UserId).Roles.Any(r=>r.RoleId==sportid))
                    {
                        result.Add(item);
                    }
                }
            }
            return new APIResult<List<SportManModel>>(result);
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
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerBindModel"></param>
        /// <returns></returns>
        [Route("Register")]
        [AllowAnonymous]
        public async Task<APIResult<AccessTokenViewModel>> Register(RegisterBindModel registerBindModel)
        {
            if (ModelState.IsValid)
            {
                var user =
                        _iSysUserService.GetAll()
                            .FirstOrDefault(u => u.UserName == registerBindModel.UserName);
                
                if(user == null)
                {
                    user = new SysUser
                    {

                        Id = Guid.NewGuid().ToString(),
                        UserName = registerBindModel.UserName,
                        Birthday= registerBindModel.Birthday,
                        FullName= registerBindModel.FullName,
                        Sex= registerBindModel.Sex,
                        SportGrade= registerBindModel.SportGrade,
                        TrainId= registerBindModel.TrainId,
                        Start4Training= registerBindModel.Start4Training,
                        TwoFactorEnabled = false,//不自动开启二次验证
                        LockoutEnabled = true,
                    };
                    user.Email = Common.CommonCodeGenerator.GenerateEmail(user.UserName);
                    IdentityResult result = await UserManager.CreateAsync(user, registerBindModel.Password);
                    if (result.Succeeded)
                    {
                        
                        if (!string.IsNullOrEmpty(registerBindModel.SysDepartmentId))
                        {
                            _iSysDepartmentSysUserService.Save(null,
                                new SysDepartmentSysUser { SysDepartmentId = registerBindModel.SysDepartmentId, SysUserId = user.Id });

                        }

                        result = await UserManager.AddToRoleAsync(user.Id, "运动员"); 
                    }
                    if (!result.Succeeded)
                    {
                        SetModelState(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", "该用户名已经存在");
                }
                if (ModelState.IsValid)
                {
                   
                    var data = await GetAccessToken(user); //登录
                    if (data != null)
                        return new APIResult<AccessTokenViewModel>(data, 0, "注册并登录成功");
                }
            }  
            return new APIResult<AccessTokenViewModel>(null, 100, "注册失败", ModelState);
        }
   
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
      
     
        /// <summary>
        /// 登录状态下更改密码 【开发完成】
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
                var roleid = user.Roles.FirstOrDefault().RoleId;
                var rolename=_isysRoleService.GetById(roleid).Name;
                return new AccessTokenViewModel(token, "bearer", user.UserName, rolename, expirers - 1);
                //}
            }
            ModelState.AddModelError("", "无效的登录尝试");
            return null;
        }
        //private class ExternalLoginData
        //{
        //    public string LoginProvider { get; set; }
        //    public string ProviderKey { get; set; }
        //    public string UserName { get; set; }

        //    public IList<Claim> GetClaims()
        //    {
        //        IList<Claim> claims = new List<Claim>();
        //        claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

        //        if (UserName != null)
        //        {
        //            claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
        //        }
        //        return claims;
        //    }

        //    public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
        //    {
        //        if (identity == null)
        //        {
        //            return null;
        //        }

        //        Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

        //        if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
        //            || String.IsNullOrEmpty(providerKeyClaim.Value))
        //        {
        //            return null;
        //        }

        //        if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
        //        {
        //            return null;
        //        }

        //        return new ExternalLoginData
        //        {
        //            LoginProvider = providerKeyClaim.Issuer,
        //            ProviderKey = providerKeyClaim.Value,
        //            UserName = identity.FindFirstValue(ClaimTypes.Name)
        //        };
        //    }
        //}

        //private static class RandomOAuthStateGenerator
        //{
        //    private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

        //    public static string Generate(int strengthInBits)
        //    {
        //        const int bitsPerByte = 8;

        //        if (strengthInBits % bitsPerByte != 0)
        //        {
        //            throw new ArgumentException("strengthInBits 必须能被 8 整除。", "strengthInBits");
        //        }

        //        int strengthInBytes = strengthInBits / bitsPerByte;

        //        byte[] data = new byte[strengthInBytes];
        //        _random.GetBytes(data);
        //        return HttpServerUtility.UrlTokenEncode(data);
        //    }
        //}
        
        #endregion
    }
}