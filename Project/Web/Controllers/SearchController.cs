using EntityFramework.Extensions;
using IServices.ICmsServices;
using IServices.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Helpers;

namespace Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ICmsArticalService _iCmsArticalService;
        private readonly ICmsCategoryService _iCmsCategoryService;
        private readonly IUnitOfWork _unitOfWork;
        public SearchController(ICmsArticalService iCmsArticalService, ICmsCategoryService iCmsCategoryService, IUnitOfWork unitOfWork)
        {
            _iCmsArticalService = iCmsArticalService;
            _iCmsCategoryService = iCmsCategoryService;
            _unitOfWork = unitOfWork;
        }
        // GET: Search
        public ActionResult Index(string keyword, int pageIndex = 1)
        {
            var model = _iCmsArticalService.GetAll(a => (a.Title.Contains(keyword) || a.Content.Contains(keyword) || a.Keywords.Contains(keyword))&& a.CmsCategory.Name != "服务机构").OrderByDescending(a => a.PublishTime).ThenByDescending(a => a.CreatedDate).ToPagedList(pageIndex);
            //ViewBag.Category = _iCmsCategoryService.GetAll(a => a.Enable && a.Name!="服务机构");
            return View(model);
        }
    }
}