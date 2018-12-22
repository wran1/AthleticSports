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
using Models.SysModels;
using IServices.ISysServices;
using Services;
using System.Data.Entity.Migrations;

namespace Web.Areas.Platform.Controllers
{
    /// <summary>
    /// Cms栏目
    /// </summary>
    public class CmsCategoryController : Controller
    {
        // GET: Platform/CmsCategory
        private readonly ICmsCategoryService _iCmsCategoryService;
        private readonly ISysAreaService _SysAreaService;
        private readonly ISysControllerService _sysControllerService;
        private readonly IUnitOfWork _unitOfWork;

        public CmsCategoryController(ICmsCategoryService iCmsCategoryService, ISysAreaService SysAreaService, ISysControllerService sysControllerService, IUnitOfWork unitOfWork)
        {
            _iCmsCategoryService = iCmsCategoryService;
            _SysAreaService = SysAreaService;
            _sysControllerService = sysControllerService;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(string keyword, string ordering, int pageIndex = 1)
        {
            var model =
                _iCmsCategoryService.GetAll()
                                 .Select(
                                     a =>
                                     new
                                     {
                                         a.Name,
                                         a.SystemId,
                                         a.CreatedDate,
                                         a.Remark,
                                         a.Id
                                     }).Search(keyword);

            if (!string.IsNullOrEmpty(ordering))
            {
                model = model.OrderBy(ordering, null);
            }
            else
            {
                model = model.OrderBy(a => a.SystemId);
            }
            return View(model.ToPagedList(pageIndex));
        }

        // 导出全部数据
        // GET: /Platform/SysHelp/Report       
        public ReportResult Report()
        {
            var model = _iCmsCategoryService.GetAll().Select(
                                     a =>
                                     new
                                     {
                                         a.Name,
                                         a.SystemId,
                                         a.CreatedDate,
                                         a.Remark,
                                         a.Id
                                     });
            var report = new Report(model.ToReportSource());

            report.DataFields["Id"].Hidden = true;
            report.TextFields.Footer = ConfigurationManager.AppSettings["Copyright"];

            return new ReportResult(report);
        }

        public ActionResult Details(object id)
        {
            var item = _iCmsCategoryService.GetById(id);
            return View(item);
        }

        public ActionResult Create(string updatedId)
        {
            return RedirectToAction("Edit", new { id = "", updatedId });
        }

        public ActionResult Edit(string id)
        {
            var item = new CmsCategory();
            if (!string.IsNullOrEmpty(id))
            {
                item = _iCmsCategoryService.GetById(id);
            }
            return View(item);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, CmsCategory collection)
        {
            var isRepeat = _iCmsCategoryService.GetAll().Any(a => a.SystemId == collection.SystemId);
            if (isRepeat && string.IsNullOrEmpty(id))
            {
                ModelState.AddModelError("SystemId", "系统编码不可重复！");
            }
            if (!ModelState.IsValid)
            {
                Edit(id);
                return View(collection);
            }
            _iCmsCategoryService.Save(id, collection);

            await _unitOfWork.CommitAsync();

            return new EditSuccessResult(id);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(object id)
        {
            _iCmsCategoryService.Delete(id);
            await _unitOfWork.CommitAsync();
            return new DeleteSuccessResult();
        }
    }
}