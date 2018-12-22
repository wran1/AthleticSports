using EntityFramework.Extensions;
using IServices.ICmsServices;
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

namespace Web.Areas.Cms.Controllers
{
    /// <summary>
    /// 资讯速递
    /// </summary>
    public class IndexController : Controller
    {
        // GET: Cms/Index
        private readonly ICmsArticalService _iCmsArticalService;
        private readonly ICmsCategoryService _iCmsCategoryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfo _iUserInfo;

        public IndexController(ICmsArticalService iCmsArticalService, ICmsCategoryService iCmsCategoryService, IUnitOfWork unitOfWork, IUserInfo iUserInfo)
        {
            _iCmsArticalService = iCmsArticalService;
            _iCmsCategoryService = iCmsCategoryService;
            _unitOfWork = unitOfWork;
            _iUserInfo = iUserInfo;
        }
        /// <summary>
        /// 资讯速递首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.ExhibitionNoticeList = _iCmsArticalService.GetAll(a=>a.CmsCategory.Name == "展览活动" && a.Enable).OrderByDescending(a => a.IsNew).OrderByDescending(a=>a.IsHot).OrderByDescending(a => a.PublishTime).Take(5).Future();
            ViewBag.ActivityNoticeList = _iCmsArticalService.GetAll(a => a.CmsCategory.Name == "泰达动态" && a.Enable).OrderByDescending(a => a.IsNew).OrderByDescending(a => a.IsHot).OrderByDescending(a => a.PublishTime).Take(5).Future();
            ViewBag.ActivityReportTop = _iCmsArticalService.GetAll(a => a.CmsCategory.Name == "活动报道" && a.Enable).OrderByDescending(a => a.IsNew).OrderByDescending(a => a.PublishTime).Take(1).Future();
            ViewBag.ActivityReportList = _iCmsArticalService.GetAll(a => a.CmsCategory.Name == "活动报道" && a.Enable).OrderByDescending(a => a.IsNew).OrderByDescending(a => a.PublishTime).Skip(1).OrderByDescending(a => a.IsNew).OrderByDescending(a => a.IsHot).OrderByDescending(a => a.PublishTime).Take(3).Future();
            ViewBag.CompanyList = _iCmsArticalService.GetAll(a => a.CmsCategory.Name == "企业采风" && a.Enable).OrderByDescending(a => a.PublishTime).Future();
            ViewBag.ZhanhuiList = _iCmsArticalService.GetAll(a => a.CmsCategory.Name == "展会现场" && a.Enable && a.IsTop).OrderByDescending(a => a.PublishTime).Take(3).Future();
            return View();
        }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pic"></param>
        /// <returns></returns>
        public ActionResult ArticleList(string tab, int pageIndex = 1, bool pic = false)
        {
            var model = _iCmsArticalService.GetAll(a => a.Enable);
            if (!string.IsNullOrEmpty(tab))
            {
                model = model.Where(a => a.CmsCategory.Name == tab);
            }
            if (pic)
            {
                return View("PicList", model.OrderByDescending(a => a.PublishTime).ThenByDescending(a=>a.CreatedDate).ToPagedList(pageIndex));
            }
            return View(model.OrderByDescending(a => a.PublishTime).ThenByDescending(a => a.CreatedDate).ToPagedList(pageIndex));
        }

        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="id"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(string id,string index)
        {
            var item = _iCmsArticalService.GetById(id);
            if (item == null || item.Deleted)
            {
                return HttpNotFound();
            }
            item.CmsArticalHits.Add(new CmsArticalHit() { CreatedBy = _iUserInfo.UserId });
            await _unitOfWork.CommitAsync();
            return View(item);
        }

        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult Share(string id, string title, string desc, string url)
        {
            return View();
        }
    }
}