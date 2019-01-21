using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BootstrapSupport;
using IServices.Infrastructure;
using IServices.ISysServices;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Models.SysModels;
using Web.Models;
using System;
using Common;
using Web.Helpers;
using IServices.IDictionaryServices;
using AutoMapper;

namespace Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ICityService _iCityService;
        private ISysDepartmentService _iDepartmentService;
        private ISysDepartmentSysUserService _iSysDepartmentSysUserService;
        //private ICompanyInfoService _iCompanyInfoService;
        public AccountController()
        {

        }

        public AccountController(ApplicationUserManager userManager, ISysDepartmentService iDepartmentService, ISysDepartmentSysUserService iSysDepartmentSysUserService,ApplicationSignInManager signInManager
            , ICityService iCityService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _iCityService = iCityService;
            _iDepartmentService = iDepartmentService;
            _iSysDepartmentSysUserService = iSysDepartmentSysUserService;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ISysDepartmentService SysDepartmentService
        {
            get
            {
                if (_iDepartmentService == null)
                    _iDepartmentService = DependencyResolver.Current.GetService<ISysDepartmentService>();
                return _iDepartmentService;
            }
        }
        public ISysDepartmentSysUserService SysDepartmentSysUserService
        {
            get
            {
                if (_iSysDepartmentSysUserService == null)
                    _iSysDepartmentSysUserService = DependencyResolver.Current.GetService<ISysDepartmentSysUserService>();
                return _iSysDepartmentSysUserService;
            }
        }
        //public IDomesticExhibitionService DomesticExhibitionService
        //{
        //    get
        //    {
        //        if (_iDomesticExhibitionService == null)
        //            _iDomesticExhibitionService = DependencyResolver.Current.GetService<IDomesticExhibitionService>();
        //        return _iDomesticExhibitionService;
        //    }
        //}
        //public IOverseasExhibitionService OverseasExhibitionService
        //{
        //    get
        //    {
        //        if (_iOverseasExhibitionService == null)
        //            _iOverseasExhibitionService = DependencyResolver.Current.GetService<IOverseasExhibitionService>();
        //        return _iOverseasExhibitionService;
        //    }
        //}

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindAsync(model.UserName, model.Password);

            if (user != null)
            {
                //var iSysEnterpriseSysUserService = DependencyResolver.Current.GetService<ISysEnterpriseSysUserService>();

                //var ents = iSysEnterpriseSysUserService.GetAll(a => a.SysUserId == user.Id);

                //if (ents.Any())
                //{
                //    if (!ents.Any(a => a.SysEnterpriseId == user.CurrentEnterpriseId))
                //    {
                //        user.CurrentEnterpriseId = ents.First().SysEnterpriseId;
                //        await UserManager.UpdateAsync(user);
                //    }
                //}
                //else
                //{
                //    ModelState.AddModelError("", "用户未绑定企业信息，请联系系统管理员！");
                //    return View(model);
                //}

                //if(user.AccountType == AccountType.企业账户)
                //{
                //    ModelState.AddModelError("", "对不起，您的账号没有访问权限！");
                //    return View(model);
                //}

                var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = model.RememberMe }, identity);

                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "用户名或密码错误！");
            return View(model);

        }

        /// <summary>
        /// 前台用户登录
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult UserLogin(string returnUrl, string UpdateTargetId)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// 前台用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <param name="UpdateTargetId"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserLogin(LoginViewModel model, string returnUrl, string UpdateTargetId)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindAsync(model.UserName, model.Password);

            if (user != null)
            {
                //if ((user.AccountType == AccountType.企业账户 && user.AuditState == AuditState.审核通过) || user.AccountType == AccountType.后台管理账户)
                //{
                //    var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                //    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = model.RememberMe }, identity);

                //    if (!string.IsNullOrEmpty(returnUrl))
                //    {
                //        return JavaScript("$('#login_modal').modal('hide');window.location.href='"+ returnUrl +"';");
                //    }
                //    return JavaScript("$('#login_modal').modal('hide');window.location.reload();");
                //}
                //else if (user.AccountType == AccountType.企业账户 && user.AuditState == AuditState.待审核)
                //{
                //    ModelState.AddModelError("", "您的账户尚未通过审核，请审核通过后再登录！");
                //    return View(model);
                //}
                //else if (user.AccountType == AccountType.企业账户 && user.AuditState == AuditState.审核未通过)
                //{
                //    ModelState.AddModelError("", "您的账户未通过审核，请重新注册账户！");
                //    return View(model);
                //}
            }
            ModelState.AddModelError("", "用户名或密码错误！");
            return View(model);
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // 要求用户已通过使用用户名/密码或外部登录名登录
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 以下代码可以防范双重身份验证代码遭到暴力破解攻击。
            // 如果用户输入错误代码的次数达到指定的次数，则会将
            // 该用户帐户锁定指定的时间。
            // 可以在 IdentityConfig 中配置帐户锁定设置
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "代码无效。");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var item = new SysUser();

            ViewBag.DepartmentId =
              SysDepartmentService.GetAll()
                  .ToSystemIdSelectList(
                      item.SysDepartmentSysUsers.FirstOrDefault(
                          /*c => c.SysDepartment.EnterpriseId == _iUserInfo.EnterpriseId*/)?.SysDepartmentId);


            var config = new MapperConfiguration(a => a.CreateMap<SysUser, RegisterViewModel>());

            var aa = config.CreateMapper().Map<RegisterViewModel>(item);

            return View(aa);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var clientIP = Request.UserHostAddress.ToString();
            if (!IPCount.CheckIsAble(clientIP))
            {
                ModelState.AddModelError("UserName", "已经超过尝试次数");
            }
            if (!ModelState.IsValid)
            {
                
                return View(model);
            }
            
            var user = new SysUser { UserName = model.UserName, Email = CommonCodeGenerator.GenerateEmail(model.UserName), };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var iUnitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();
                if (!string.IsNullOrEmpty(model.DepartmentId))
                {
                    SysDepartmentSysUserService.Save(null,
                        new SysDepartmentSysUser { SysDepartmentId = model.DepartmentId, SysUserId = user.Id });
                 
                }
                //前三个用户赋予管理员权限 

                if (UserManager.Users.Count() < 3)
                {
                 
                    await UserManager.AddToRoleAsync(user.Id, "系统管理员");

                }
                else
                {
                    await UserManager.AddToRoleAsync(user.Id, "运动员");
                }
                await iUnitOfWork.CommitAsync();
                TempData[Alerts.Success] = "注册成功，请您登陆";

                //await SignInManager.SignInAsync(user, false, true);

                // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
                // 发送包含此链接的电子邮件
                //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "确认你的帐户", "请通过单击 <a href=\"" + callbackUrl + "\">这里</a>来确认你的帐户");

                return RedirectToAction("Index", "Index", new { area = "Platform" });
            }
            AddErrors(result);


            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        /// <summary>
        /// 用户声明
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult UserDeclaration()
        {
            return View();
        }

        /// <summary>
        /// 同意用户声明
        /// </summary>
        /// <param name="isAgree"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UserDeclaration(bool isAgree = false)
        {
            if (isAgree)
            {
                return RedirectToAction("UserRegister");
            }
            return View();
        }

        /// <summary>
        /// 前台用户注册
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        //public ActionResult UserRegister()
        //{
        //    var item = new UserRegisterModel();
        //    ViewBag.IndustryOwnedId = new SelectList(IndustryOwnedService.GetAll(a => a.Enable).OrderBy(a => a.SystemId).Select(a => new { a.Id, a.Name }), "Id", "Name", item.IndustryOwnedId);
        //    ViewBag.DomesticExhibitionId = new SelectList(DomesticExhibitionService.GetAll(a => a.Enable).OrderBy(a => a.SystemId).Select(a => new { a.Id, a.Name }), "Id", "Name", item.DomesticExhibitionId);
        //    ViewBag.OverseasExhibitionId = new SelectList(OverseasExhibitionService.GetAll(a => a.Enable).OrderBy(a => a.SystemId).Select(a => new { a.Id, a.Name }), "Id", "Name", item.OverseasExhibitionId);
        //    return View(item);
        //}

        /// <summary>
        /// 前台用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> UserRegister(UserRegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new SysUser { UserName = model.UserName, Email = model.Email, FullName = model.FullName,Sex = model.Sex, PhoneNumber = model.PhoneNumber, AccountType = AccountType.企业账户 };
        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            user.CompanyInfo = new CompanyInfo
        //            {
        //                CompanyName = model.CompanyName,
        //                EfCompanyName = model.EfCompanyName,
        //                CompanyType = model.CompanyType,
        //                CompanyUrl = model.CompanyUrl,
        //                MemberType = model.MemberType,
        //                TotalAsset = model.TotalAsset,
        //                IsExportExperience = model.IsExportExperience,
        //                TotalStaff = model.TotalStaff,
        //                AnnuaSales = model.AnnuaSales,
        //                RegisteredAddress = model.RegisteredAddress,
        //                ActualAddress = model.ActualAddress,
        //                Keywords = model.Keywords,
        //                Keyword4English = model.Keyword4English,
        //                IndustryOwnedId = IndustryOwnedService.GetIdBySystemId(model.IndustryOwnedId),
        //                MainBusiness = model.MainBusiness,
        //                MainBusiness4English = model.MainBusiness4English,
        //                Registered = true,
        //                IsExhibitionIntention = model.IsExhibitionIntention,
        //                DomesticExhibitionId = model.DomesticExhibitionId == null ? null : DomesticExhibitionService.GetIdBySystemId(model.DomesticExhibitionId),
        //                OverseasExhibitionId = model.OverseasExhibitionId == null ? null : OverseasExhibitionService.GetIdBySystemId(model.OverseasExhibitionId),
        //                OtherExhibition = model.OtherExhibition,
        //            };
        //            user.PersonalInfo = new PersonalInfo
        //            {
        //                EnglishName = model.EnglishName,
        //                WxId = model.WxId,
        //                Cposition = model.Cposition,
        //                Eposition = model.Eposition,
        //                FixedPhone = model.FixedPhone,
        //                Fox = model.Fox,
        //                IsPublicPhone = model.IsPublicPhone,
        //                IsAgreeUserDeclaration = true,
        //            };
        //            await UserManager.UpdateAsync(user);
        //            return Content("<script>alert('注册成功！请等待后台审核，现在将返回首页...');location.href='/Home/Home';</script>");
        //        }
        //        AddErrors(result);
        //    }
        //    // 如果我们进行到这一步时某个地方出错，则重新显示表单
        //    return View(model);
        //}
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var clientIP = Request.UserHostAddress.ToString();
            if (!IPCount.CheckIsAble(clientIP))
            {
                ModelState.AddModelError("UserName", "已经超过尝试次数");
            }
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    // 请不要显示该用户不存在或者未经确认
                    ModelState.AddModelError("", "用户不存在！");
                }
                else
                {
                    var useremail = user.Email.Substring(0, 2) + "***" + user.Email.Substring(user.Email.IndexOf('@'));
                    //身份验证
                    var preVerify4ResetPasswordModel = new PreVerify4ResetPasswordModel {
                        UserName = user.UserName,
                        Email = useremail
                    };
                    return View("Verify4ResetPassword", preVerify4ResetPasswordModel);//跳转到身份认证
                }
                //var result4VerifyCode =
                // await PhoneVerifyCodeProvider.CodeVerify(user.PhoneNumber, model.VerifyCode, TimeSpan.Zero);

                //if (!result4VerifyCode.Success)//验证码验证
                //{
                //    ModelState.AddModelError("VerifyCode", "验证码错误！");
                //}

                //var aa = await UserManager.PasswordValidator.ValidateAsync(model.Password);
                //if (!aa.Succeeded)
                //{
                //    foreach (var error in aa.Errors)
                //    {
                //        ModelState.AddModelError("", error);
                //    }
                //}

                //if (!ModelState.IsValid)
                //{
                //    return View(model);
                //}

                //await UserManager.RemovePasswordAsync(user.Id);

                //await UserManager.AddPasswordAsync(user.Id, model.Password);

                //return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        /// <summary>
        /// 忘记密码身份验证
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Verify4ResetPassword()
        {
            return RedirectToAction("ForgotPassword");
        }
        /// <summary>
        /// 忘记密码身份验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Verify4ResetPassword(PreVerify4ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    return RedirectToAction("ForgotPassword");
                }
                //验证身份

                var success = false;

                #region: 验证码验证

                var result4VerifyCode =
                        await
                            EmailVerifyCodeProvider.CodeVerify(user.Email, model.VerifyCode,
                                TimeSpan.FromMinutes(30));
                if (!result4VerifyCode.Success)
                {
                    foreach (var err in result4VerifyCode.ErroMessage)
                    {
                        ModelState.AddModelError(err.Key, err.Value);
                    }
                }
                else
                {
                    success = true;
                }

                #endregion

                if (success)
                {
                    //生成重置密码的Code
                    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    return View("ResetPassword", new ResetPasswordModel { Code = code, UserId = user.Id }); //跳转到密码重置}
                }
            }
            return View(model);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return RedirectToAction("ForgotPassword");
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                //1 验证Code并重置密码
                var result = await UserManager.ResetPasswordAsync(model.UserId, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return Content("<script>alert('重置密码成功！请重新登录，现在将返回首页...');location.href='/Home/Home';</script>");
                    //return RedirectToAction("Login", "Account", new { area = "" }); //跳转到登录页
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err);
                }
            }
            return View(model);
        }


        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 请求重定向到外部登录提供程序
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // 生成令牌并发送该令牌
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // 如果用户已具有登录名，则使用此外部登录提供程序将该用户登录
            var result = await SignInManager.ExternalSignInAsync(loginInfo, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // 如果用户没有帐户，则提示该用户创建帐户
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                // 从外部登录提供程序获取有关用户的信息
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new SysUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, false, false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        /// <summary>
        /// 后台注销
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Index", new { area = "Platform" });
        }

        /// <summary>
        /// 前台注销
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult UserLogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        [HttpGet] // 只能用GET ！！！
        [AllowAnonymous]
        public async Task<ActionResult> CheckUserAccountExists(string userName)
        {
            var exists = await UserManager.FindByNameAsync(userName);

            //string[] existsUsers = { "youguanbumen", "wodanwojun" };
            //bool exists = string.IsNullOrEmpty(existsUsers.FirstOrDefault(u => u.ToLower() == userAccount.ToLower())) == false;
            return Json(exists == null, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet] // 只能用GET ！！！
        //[AllowAnonymous]
        //public async Task<ActionResult> CheckCompanyNameExists(string CompanyName)
        //{
        //    var exists = _iCompanyInfoService.GetAll().Any(a=>a.CompanyName== CompanyName && a.Registered);
        //    return Json(exists, JsonRequestBehavior.AllowGet);
        //}

        #region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        /// <summary>
        /// 忘记密码发送验证码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetVerifyCode4ResetPassword(string userName, string email)
        {
            JsonResultWithErro<bool> resultData = new JsonResultWithErro<bool>(true);
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                #region 发送验证码

                if (!string.IsNullOrEmpty(user.Email))
                {
                    var verifyCode = CommonCodeGenerator.Generator(6);
                    var verifyEmail = user.Email;
                    var result4Send = await EmailVerifyCodeProvider.SendCode(verifyCode, verifyEmail, "您的验证码为 ");
                    if (!result4Send.Success)
                    {
                        resultData.Data = false;
                        resultData.StateCode = 101; //加载错误消息
                        resultData.Message = string.Join(";", result4Send.ErroMessage.Values);
                    }
                }
                else
                {
                    resultData.Data = false;
                    resultData.StateCode = 18;
                    resultData.Message = "您还没有填写邮箱";
                }

                #endregion
            }
            else
            {
                resultData.Data = false;
                resultData.StateCode = 20;
                resultData.Message = "用户不存在，获取验证码失败";
            }

            return Json(resultData, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    /// <summary>
    /// 带错误信息的返回类 0 操作成功 1-100 显示提示信息 100以上错误信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonResultWithErro<T>
    {
        public JsonResultWithErro(T data, int stateCode = 0, string message = "操作成功")
        {
            Data = data;
            StateCode = stateCode;
            Message = message;
        }


        /// <summary>
        /// 正常返回的数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 状态码
        /// 0为成功，Message为操作成功的提示信息，
        /// 1-99需要下一步操作，在返回值中定义并说明
        /// 100直接显示错误信息
        /// </summary>
        public int StateCode { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
    }
}