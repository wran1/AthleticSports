using EntityFramework.Extensions;
using IServices.IWebsiteManagementServices;
using IServices.Infrastructure;
using IServices.ISysServices;
using Models.SysModels;
using Models.WebsiteManagement;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web.Areas.Platform.Helpers;   
using Web.Helpers;

namespace Web.Controllers
{
    /// <summary>
    /// 留言板
    /// </summary>
    public class MessageBoardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfo _iUserInfo;
        //public MessageBoardController(IConsultationService iConsultationService, IUnitOfWork unitOfWork, IUserInfo iUserInfo)
        //{
        //    _iConsultationService = iConsultationService;
        //    _unitOfWork = unitOfWork;
        //    _iUserInfo = iUserInfo;
        //}
        //// GET: Consultation
        //public ActionResult Index(int pageIndex = 1)
        //{
        //    var model = new Consultation();
        //    ViewBag.Consultation = _iConsultationService.GetAll(a => !a.Deleted && a.AuditState == AuditState.审核通过).ToPagedList(pageIndex, 10);      
        //    return View(model);
        //}
        //[HttpPost]
        //[ValidateInput(false)]
        //public async Task<ActionResult> Index(Consultation collection)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        Index();
        //        return View(collection);
        //    }
        //    _iConsultationService.Save(null, collection);
        //    await _unitOfWork.CommitAsync();
        //    return Content("<script>alert('提交成功！请等待后台审核与回复！');window.location.href='/MessageBoard';</script>");
        //}
    }
}