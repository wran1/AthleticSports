using IServices.IDictionaryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class GetDatasController : Controller
    {
        //private readonly IIndustryOwnedService _iIndustryOwnedService;
        //private readonly IDomesticExhibitionService _iDomesticExhibitionService;
        //private readonly IOverseasExhibitionService _iOverseasExhibitioService;
        //public GetDatasController(IIndustryOwnedService iIndustryOwnedService, IDomesticExhibitionService iDomesticExhibitionService, IOverseasExhibitionService iOverseasExhibitioService)
        //{
        //    _iIndustryOwnedService = iIndustryOwnedService;
        //    _iDomesticExhibitionService = iDomesticExhibitionService;
        //    _iOverseasExhibitioService = iOverseasExhibitioService;
        //}
        //// GET: GetDatas
        //[AllowAnonymous]
        //public JsonResult Industrys(string systemId)
        //{
        //    if (string.IsNullOrEmpty(systemId))
        //    {
        //        systemId = "";
        //    }
        //    var industrys = _iIndustryOwnedService.GetAll(d=>!d.Deleted).Where(d => d.SystemId.StartsWith(systemId) && d.SystemId.Length == systemId.Length + 3).Select(d => new { d.SystemId, d.Name }).OrderBy(d => d.SystemId).ToList();
        //    return Json(industrys, JsonRequestBehavior.AllowGet);
        //}
        //[AllowAnonymous]
        //public JsonResult DomesticExhibitions(string systemId)
        //{
        //    if (string.IsNullOrEmpty(systemId))
        //    {
        //        systemId = "";
        //    }
        //    var DomesticExhibitions = _iDomesticExhibitionService.GetAll(d => !d.Deleted).Where(d => d.SystemId.StartsWith(systemId) && d.SystemId.Length == systemId.Length + 3).Select(d => new { d.SystemId, d.Name }).OrderBy(d => d.SystemId).ToList();
        //    return Json(DomesticExhibitions, JsonRequestBehavior.AllowGet);
        //}
        //[AllowAnonymous]
        //public JsonResult OverseasExhibitions(string systemId)
        //{
        //    if (string.IsNullOrEmpty(systemId))
        //    {
        //        systemId = "";
        //    }
        //    var OverseasExhibitions = _iOverseasExhibitioService.GetAll(d => !d.Deleted).Where(d => d.SystemId.StartsWith(systemId) && d.SystemId.Length == systemId.Length + 3).Select(d => new { d.SystemId, d.Name }).OrderBy(d => d.SystemId).ToList();
        //    return Json(OverseasExhibitions, JsonRequestBehavior.AllowGet);
        //}
    }
}