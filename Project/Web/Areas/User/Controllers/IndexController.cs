using AutoMapper;
using BootstrapSupport;
using IServices.IDictionaryServices;
using IServices.IWebsiteManagementServices;
using IServices.IMessage;
using IServices.Infrastructure;
using IServices.ISysServices;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Models.SysModels;
using Models.WebsiteManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Platform.Helpers;
using Web.Areas.User.Models;
using Web.Helpers;

namespace Web.Areas.User.Controllers
{
    /// <summary>
    /// 个人中心
    /// </summary>
    public class IndexController : Controller
    {
        // GET: User/Index
        private readonly ISysUserService _sysUserService;
        private readonly IUserInfo _iUserInfo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISysBroadcastService _iSysBroadcastService;
        //private readonly ICompanyInfoService _iCompanyInfoService;
        public IndexController(ISysBroadcastService iSysBroadcastService,ISysUserService sysUserService, IUnitOfWork unitOfWork, IUserInfo iUserInfo
            )
        {
            _sysUserService = sysUserService;
            _unitOfWork = unitOfWork;
            _iUserInfo = iUserInfo;
            _iSysBroadcastService = iSysBroadcastService;         
            //_iCompanyInfoService = iCompanyInfoService;
        }

        public ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }
        public ApplicationSignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
        }

        /// <summary>
        /// 消息提醒
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int pageIndex = 1)
        { 
            var model = _iSysBroadcastService.GetAll(t => t.AddresseeId == _iUserInfo.UserId).OrderByDescending(t=>t.CreatedDate);
            return View(model.ToPagedList(pageIndex));
        }

        /// <summary>
        /// 账号及密码
        /// </summary>
        /// <returns></returns>
        public ActionResult Account()
        {
            var user = _sysUserService.GetById(_iUserInfo.UserId);
            return View(user);
        }

        /// <summary>
        /// 账号及密码设置
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountSetting()
        {
            var model = new AccountEditModel();
            return View(model);
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccountSetting(AccountEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.CurrentPassword, model.Password);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return Content("<script>alert('密码修改成功！');location.href='/User/Index/Account';</script>");
                    }
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Replace("Incorrect password", "密码不正确"));
                    }
                }
            }
            return View(model);
        }

        ///// <summary>
        ///// 资料管理
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult DataManagement()
        //{
        //    var item = _sysUserService.GetById(_iUserInfo.UserId);
        //    var config = new MapperConfiguration(a => a.CreateMap<SysUser, DataEditModel>());
        //    var mapper = config.CreateMapper();
        //    var model = mapper.Map<DataEditModel>(item);
        //    if (item == null)
        //    {
        //        return Content("<script>alert('此页面需登录后再访问,现在将跳转回首页。。');location.href='/Home/Home';</script>");
        //    }
        //    if (item.PersonalInfo != null)
        //    {
        //        model.EnglishName = item.PersonalInfo.EnglishName;
        //        model.Cposition = item.PersonalInfo.Cposition;
        //        model.Eposition = item.PersonalInfo.Eposition;
        //        model.FixedPhone = item.PersonalInfo.FixedPhone;
        //        model.Fox = item.PersonalInfo.Fox;
        //        model.IsPublicPhone = item.PersonalInfo.IsPublicPhone;
        //        model.WxId = item.PersonalInfo.WxId;
        //    }
        //    if (item.CompanyInfo != null)
        //    {
        //        model.CompanyName = item.CompanyInfo.CompanyName;
        //        model.EfCompanyName = item.CompanyInfo.EfCompanyName;
        //        model.RegisteredAddress = item.CompanyInfo.RegisteredAddress;
        //        model.ActualAddress = item.CompanyInfo.ActualAddress;
        //        model.CompanyType = item.CompanyInfo.CompanyType;
        //        model.MemberType = item.CompanyInfo.MemberType;
        //        model.CompanyUrl = item.CompanyInfo.CompanyUrl;
        //        model.TotalAsset = item.CompanyInfo.TotalAsset;
        //        model.IsExportExperience = item.CompanyInfo.IsExportExperience;
        //        model.TotalStaff = item.CompanyInfo.TotalStaff;
        //        model.AnnuaSales = item.CompanyInfo.AnnuaSales;
        //        model.Keywords = item.CompanyInfo.Keywords;
        //        model.Keyword4English = item.CompanyInfo.Keyword4English;
        //        model.IndustryOwnedId = _iIndustryOwnedService.GetSystemIdById(item.CompanyInfo.IndustryOwnedId);
        //        model.MainBusiness = item.CompanyInfo.MainBusiness;
        //        model.MainBusiness4English = item.CompanyInfo.MainBusiness4English;
        //        model.IsExhibitionIntention = item.CompanyInfo.IsExhibitionIntention;
        //        model.DomesticExhibitionId = item.CompanyInfo.DomesticExhibitionId == null ? null : _iDomesticExhibitionService.GetSystemIdById(item.CompanyInfo.DomesticExhibitionId);
        //        model.OverseasExhibitionId = item.CompanyInfo.OverseasExhibitionId == null ? null : _iOverseasExhibitionService.GetSystemIdById(item.CompanyInfo.OverseasExhibitionId);
        //        model.OtherExhibition = item.CompanyInfo.OtherExhibition;
        //    }
        //    return View(model);
        //}

        ///// <summary>
        ///// 修改资料
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DataManagement(DataEditModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByIdAsync(_iUserInfo.UserId);
        //        if (user != null)
        //        {
        //            user.FullName = model.FullName;
        //            user.Sex = model.Sex;
        //            user.Email = model.Email;
        //            user.PhoneNumber = model.PhoneNumber;
        //            user.CompanyInfo.TotalStaff = model.TotalStaff;
        //            user.CompanyInfo.Keywords = model.Keywords;
        //            user.CompanyInfo.Keyword4English = model.Keyword4English;
        //            user.CompanyInfo.CompanyUrl = model.CompanyUrl;
        //            user.CompanyInfo.IsExhibitionIntention = model.IsExhibitionIntention;
        //            user.CompanyInfo.DomesticExhibitionId = (model.DomesticExhibitionId == null ? null : _iDomesticExhibitionService.GetIdBySystemId(model.DomesticExhibitionId));
        //            user.CompanyInfo.OverseasExhibitionId = (model.OverseasExhibitionId == null ? null : _iOverseasExhibitionService.GetIdBySystemId(model.OverseasExhibitionId));
        //            user.CompanyInfo.OtherExhibition = model.OtherExhibition;
        //            user.PersonalInfo.EnglishName = model.EnglishName;
        //            user.PersonalInfo.WxId = model.WxId;
        //            user.PersonalInfo.Cposition = model.Cposition;
        //            user.PersonalInfo.Eposition = model.Eposition;
        //            user.PersonalInfo.FixedPhone = model.FixedPhone;
        //            user.PersonalInfo.Fox = model.Fox;
        //            user.PersonalInfo.IsPublicPhone = model.IsPublicPhone;
        //            await UserManager.UpdateAsync(user);
        //            return Content("<script>alert('保存成功！');location.href='/User/Index/DataManagement';</script>");
        //        }
        //    }
        //    return View(model);
        //}

        ///// <summary>
        ///// 公司介绍
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult CompanyIntroduce()
        //{
        //    var item = _sysUserService.GetById(_iUserInfo.UserId);
        //    if(item == null)
        //    {
        //        return Content("<script>alert('此页面需登录后再访问,现在将跳转回首页。。');location.href='/Home/Home';</script>");
        //    }
        //    var config = new MapperConfiguration(a => a.CreateMap<CompanyInfo, CompanyIndroduceEditModel>());
        //    var mapper = config.CreateMapper();
        //    var model = mapper.Map<CompanyIndroduceEditModel>(item.CompanyInfo);
        //    return View(model);
        //}

        ///// <summary>
        ///// 保存公司介绍
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> CompanyIntroduce(CompanyIndroduceEditModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByIdAsync(_iUserInfo.UserId);
        //        if (user != null)
        //        {
        //            user.CompanyInfo.LogoUrl = model.LogoUrl;
        //            user.CompanyInfo.MainProduct = model.MainProduct;
        //            user.CompanyInfo.ChineseIntroduction = model.ChineseIntroduction;
        //            user.CompanyInfo.EnglishIntroduction = model.EnglishIntroduction;
        //            await UserManager.UpdateAsync(user);
        //            return Content("<script>alert('保存成功！');location.href='/User/Index/CompanyIntroduce';</script>");
        //        }
        //    }
        //    return View(model);
        //}
        //public ActionResult ProductEdit(string id)
        //{
        //    var model = _iProductService.GetById(id);
        //    ViewBag.productownedName = model.CompanyInfo?.IndustryOwned?.FullName;
        //    return View(model);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ProductEdit(Product collection, string id)
        //{
           
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //            var model = _iProductService.GetById(id);
        //            model.ProductImgUrl = collection.ProductImgUrl;
        //            model.Version = collection.Version;
        //            model.Description = collection.Description;
        //            model.ProductName = collection.ProductName;
        //            _iProductService.Save(id, model);
        //            await _unitOfWork.CommitAsync();
        //        return Content("<script>alert('保存成功！');location.href='/User/Index/ProductManagement';</script>");
        //    }
        //    return View(collection);

        //}
        ///// <summary>
        ///// 产品管理
        ///// </summary>
        ///// <returns></returns>

        //public ActionResult ProductManagement()
        //{
        //    return View();
        //}
        //[HttpDelete]
        //public async Task<ActionResult> ProductDelete(string id)
        //{
        //    _iProductService.Delete(id);
        //    await _unitOfWork.CommitAsync();
        //    return new DeleteSuccessResult();
        //}
        //public ActionResult ProductList(int pageIndex=1)
        //{
        //    var CompanyInfoId = _sysUserService.GetAll(a => a.Id == _iUserInfo.UserId && !a.Deleted && a.AuditState == AuditState.审核通过).FirstOrDefault()?.CompanyInfoId;
        //    var model = _iProductService.GetAll(a => a.CompanyInfoId == CompanyInfoId).OrderByDescending(a => a.CreatedDate);
        //    return View(model.ToPagedList(pageIndex));
        //}
        //public ActionResult ProductCreat()
        //{
        //    var CompanyInfoModel = _sysUserService.GetAll(a => a.Id == _iUserInfo.UserId && !a.Deleted && a.AuditState == AuditState.审核通过);
        //    var model = new Product();
        //    model.CompanyInfoId = CompanyInfoModel.Where(t=>t.AuditState== AuditState.审核通过).FirstOrDefault()?.CompanyInfoId ;
        //   ViewBag.productownedName = CompanyInfoModel.FirstOrDefault()?.CompanyInfo?.IndustryOwned?.FullName;
        //    return View(model);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ProductCreat(Product collection)
        //{
        //    if(string.IsNullOrEmpty(collection.CompanyInfoId))
        //    {
        //        return Content("<script>alert('请先填写公司信息或等待公司信息审核通过！');location.href='/User/Index/ProductCreat';");
        //    }
          
        //    if (ModelState.IsValid)
        //    {
        //            _iProductService.Save(null, collection);
        //            await _unitOfWork.CommitAsync(); 
        //        return Content("<script>alert('保存成功！');location.href='/User/Index/ProductManagement';</script>");
        //    }
        //    return View(collection);
        
        //}
    }
}