using Common;
using IServices.ITestServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Helpers;

namespace Web.Areas.Platform.Controllers
{
    public class ExportRecordController : Controller
    {
        private readonly IExportRecordService _exportRecordService;

        public ExportRecordController(IExportRecordService exportRecordService)
        {
            _exportRecordService = exportRecordService;
        }
        // GET: Platform/ExportRecord
        public async Task<ActionResult> Index(string keyword, string ordering, int pageIndex = 1)
        {
            var model =
                _exportRecordService.GetAll()
                                  .Select(
                                      a =>
                                      new
                                      {
                                          a.SysArea,
                                          a.UserCreatedBy.UserName,
                                          a.CreatedDate
                                      }).Search(keyword);

            if (!string.IsNullOrEmpty(ordering))
            {
                model = model.OrderBy(ordering, null);
            }

            return View(model.ToPagedList(pageIndex));
        }

    }
}