using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Common;
using DoddleReport;
using DoddleReport.Web;
using IServices.Infrastructure;
using Web.Helpers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Web.Areas.Platform.Helpers;
using IServices.ICmsServices;
using Models.CmsModels;

namespace Web.Areas.Platform.Controllers
{
    /// <summary>
    /// Cms文章
    /// </summary>
    public class CmsArticalController : Controller
    {
        // GET: Platform/CmsArtical
        private readonly ICmsArticalService _iCmsArticalService;
        private readonly ICmsCategoryService _iCmsCategoryService;
        private readonly IUnitOfWork _unitOfWork;

        public CmsArticalController(ICmsArticalService iCmsArticalService, ICmsCategoryService iCmsCategoryService, IUnitOfWork unitOfWork)
        {
            _iCmsArticalService = iCmsArticalService;
            _iCmsCategoryService = iCmsCategoryService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="ordering"></param>
        /// <param name="parameter">栏目SystemId</param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Index(string keyword, string ordering, string parameter, int pageIndex = 1)
        {
            var model =
                _iCmsArticalService.GetAll(a=>a.CmsCategory.SystemId.Substring(0,3) == parameter && a.CmsCategory.Enable)
                                 .Select(
                                     a =>
                                     new
                                     {
                                         a.Title,
                                         CmsCategoryName = a.CmsCategory.Name,
                                         a.Author,
                                         a.PublishTime,
                                         a.Enable,
                                         a.IsTop,
                                         a.IsHot,
                                         a.IsNew,
                                         HitCount = a.CmsArticalHits.Count(),
                                         CreatedBy = a.UserCreatedBy.UserName,
                                         a.Id,
                                
                                     }).Search(keyword);

            if (!string.IsNullOrEmpty(ordering))
            {
                model = model.OrderBy(ordering, null);
            }
            else
            {
                model = model.OrderByDescending(a=>a.PublishTime);
            }
            return View(model.ToPagedList(pageIndex));
        }

        // 导出全部数据
        // GET: /Platform/SysHelp/Report       
        public ReportResult Report(string parameter)
        {
            var model = _iCmsArticalService.GetAll(a => a.CmsCategoryId == parameter && a.CmsCategory.Enable).Select(
                                     a =>
                                     new
                                     {
                                         a.Title,
                                         CmsCategoryName = a.CmsCategory.Name,
                                         a.Author,
                                         a.PublishTime,
                                         HitCount = a.CmsArticalHits.Count(),
                                         CreatedBy = a.UserCreatedBy.UserName,
                                         a.Id
                                     });
            var report = new Report(model.ToReportSource());

            report.DataFields["Id"].Hidden = true;
            report.TextFields.Footer = ConfigurationManager.AppSettings["Copyright"];

            return new ReportResult(report);
        }

        public ActionResult Details(object id)
        {
            var item = _iCmsArticalService.GetById(id);
            ViewBag.CmsCategoryId = item.CmsCategory?.Name;
            return View(item);
        }

        public ActionResult Create(string updatedId,string parameter)
        {
            return RedirectToAction("Edit", new { id = "", updatedId, parameter });
        }

        public ActionResult Edit(string id, string parameter)
        {
            var item = new CmsArtical();
            if (!string.IsNullOrEmpty(id))
            {
                item = _iCmsArticalService.GetById(id);
            }
            ViewBag.Category = _iCmsCategoryService.GetAll(a => a.Enable && a.SystemId == parameter).FirstOrDefault();
            ViewBag.CmsCategoryId = new SelectList(_iCmsCategoryService.GetAll(a => a.Enable && a.SystemId.Substring(0, 3) == parameter && a.SystemId.Length > 3).Select(a => new { a.Id, a.Name }),
                "Id", "Name", item.CmsCategoryId);
            return View(item);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(string id, string parameter, CmsArtical collection)
        {
            if (!ModelState.IsValid)
            {
                Edit(id, parameter);
                return View(collection);
            }
            if (string.IsNullOrEmpty(collection.CmsCategoryId))
            {
                var categoryId = _iCmsCategoryService.GetAll(a => a.SystemId == parameter).Select(a => a.Id).FirstOrDefault();
                collection.CmsCategoryId = categoryId;
            }

            _iCmsArticalService.Save(id, collection);

            await _unitOfWork.CommitAsync();

            return new EditSuccessResult(id);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(object id, string parameter)
        {
            _iCmsArticalService.Delete(id);
            await _unitOfWork.CommitAsync();
            return new DeleteSuccessResult();
        }
    }
}