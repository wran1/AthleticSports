using Common;
using IServices.ICmsServices;
using IServices.Infrastructure;
using IServices.IWebsiteManagementServices;
using Models.SysModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Helpers;

namespace Web.Areas.WebsitePage.Controllers
{
    public class SearchController : Controller
    {
        private readonly ICmsArticalService _iCmsArticalService;
        private readonly ICmsCategoryService _iCmsCategoryService;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IProductService _iProductService;
        public SearchController( ICmsArticalService iCmsArticalService, ICmsCategoryService iCmsCategoryService, IUnitOfWork unitOfWork)
        {
            _iCmsArticalService = iCmsArticalService;
            _iCmsCategoryService = iCmsCategoryService;
            _unitOfWork = unitOfWork;
        }
        // GET: WebsitePage/Search
        public ActionResult Index(string keyword,string type,int pageIndex=1)
        {
            var model = _iCmsArticalService.GetAll(a=>a.Enable);
            if(!string.IsNullOrEmpty(keyword))
            {
                model = model.Search(keyword);
            }
            if(!string.IsNullOrEmpty(type))
            {
                if(type=="会展通知")
                {
                    model = model.Where(a => a.CmsCategory.Name == "会展通知");
                }
                else if(type == "泰达动态")
                {
                    model = model.Where(a => a.CmsCategory.Name == "泰达动态");
                }
                else if (type == "会展现场")
                {
                    model = model.Where(a => a.CmsCategory.Name == "会展现场");
                }
                else if (type == "活动报道")
                {
                    model = model.Where(a => a.CmsCategory.Name == "活动报道");
                }
                else if (type == "企业风采")
                {
                    model = model.Where(a => a.CmsCategory.Name == "企业风采");
                }
            }
            model = model.OrderByDescending(a => a.PublishTime).ThenByDescending(a => a.CreatedDate);
            ViewBag.CmsArtical = model.Count();
            return View(model.ToPagedList(pageIndex));
        }
        //public ActionResult ChanpinSearch(string keyword,string type,string industryId, int pageIndex = 1)
        //{
        //    var productmodel = _iProductService.GetAll(a => a.AuditState == AuditState.审核通过);
           
        //    if (!string.IsNullOrEmpty(industryId))
        //    {
        //        productmodel = productmodel.Where(a => a.CompanyInfo.IndustryOwned.SystemId.StartsWith(industryId));
        //    }
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        productmodel = productmodel.Where(a => a.ProductName.Contains(keyword));
        //    }
        //    if(!string.IsNullOrEmpty(type))
        //    {
        //        if(type=="new")
        //        {
        //            productmodel = productmodel.Where(a => a.Newproduct);
        //        }
        //        else if(type=="last")
        //        {
        //            productmodel = productmodel.Where(a => a.Lastproduct);
        //        }
        //    }
        //    ViewBag.Count = productmodel.Count();
        //    ViewBag.Keyword = keyword;
        //    ViewBag.Type = type;
        //    return View(productmodel.ToPagedList(pageIndex));
        //}
    }
}