using EntityFramework.Extensions;
using IServices.ICmsServices;
using IServices.IDictionaryServices;
using IServices.IWebsiteManagementServices;
using IServices.Infrastructure;
using IServices.ISysServices;
using Models.CmsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Helpers;
using Models.Dictionary;
using Models.SysModels;
using Models.WebsiteManagement;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICmsArticalService _iCmsArticalService;
        private readonly ICmsCategoryService _iCmsCategoryService;
        //private readonly ICompanyInfoService _iCompanyInfoService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfo _iUserInfo;
        public HomeController(ICmsArticalService iCmsArticalService, ICmsCategoryService iCmsCategoryService,
           IUnitOfWork unitOfWork, IUserInfo iUserInfo)
        {
            _iCmsArticalService = iCmsArticalService;
            _iCmsCategoryService = iCmsCategoryService;
            //_iCompanyInfoService = iCompanyInfoService;
            _unitOfWork = unitOfWork;
            _iUserInfo = iUserInfo;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Index", new { area = "Platform" });
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            return View();
        }

    
    }
}
