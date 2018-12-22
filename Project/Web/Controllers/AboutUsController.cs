using IServices.IWebsiteManagementServices;
using IServices.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.WebsiteManagement;
using IServices.ISysServices;
using System.Threading.Tasks;
using Web.Helpers;

namespace Web.Controllers
{
    /// <summary>
    /// 关于我们
    /// </summary>
    public class AboutUsController : Controller
    {
        // GET: AboutUs
        //private readonly IAnnualPlanService _iAnnualPlanService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfo _iUserInfo;

        public AboutUsController( IUnitOfWork unitOfWork, IUserInfo iUserInfo)
        {
            //_iAnnualPlanService = iAnnualPlanService;
            _unitOfWork = unitOfWork;
            _iUserInfo = iUserInfo;
        }
        /// <summary>
        /// 中心简介
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        ///// <summary>
        ///// 年度计划
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ProfessionalService(int pageIndex = 1)
        //{
        //    var model = _iAnnualPlanService.GetAll(a => a.Enable).OrderByDescending(a => a.ActivityStartTime).ToPagedList(pageIndex);
        //    return View(model);
        //}
        ///// 专业服务
        //public ActionResult ZhuanService()
        //{
        //    return View();
        //}
    }
}