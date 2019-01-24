using IServices.Infrastructure;
using IServices.ISysServices;
using IServices.ITestServices;
using Models.SysModels;
using Models.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Platform.Helpers;

namespace Web.Areas.Platform.Controllers
{
    public class TrainingPeopleController : Controller
    {
        // GET: Platform/TrainingPeople
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserInfo _iUserInfo;
        private readonly ITrainingTypeService _trainingTypeService;
        private readonly ITrainingRelationService _trainingRelationService;
        private readonly ISysDepartmentSysUserService _iSysDepartmentSysUserService;
        private readonly ISysDepartmentService _iDepartmentService;
        public TrainingPeopleController(ISysDepartmentService iDepartmentService,IUnitOfWork unitOfWork, IUserInfo iUserInfo, ITrainingTypeService trainingTypeService, ITrainingRelationService trainingRelationService, ISysDepartmentSysUserService iSysDepartmentSysUserService)
        {
            _unitOfWork = unitOfWork;
            _iUserInfo = iUserInfo;
            _trainingTypeService = trainingTypeService;
            _trainingRelationService = trainingRelationService;
            _iSysDepartmentSysUserService = iSysDepartmentSysUserService;
            _iDepartmentService = iDepartmentService;
        }
        public ActionResult Index()
        {
            return RedirectToAction("Edit");
        }
        public ActionResult Edit()
        {
            var item = new SysDepartment();
            var Depart = _iSysDepartmentSysUserService.GetAll(a => a.SysUserId == _iUserInfo.UserId).FirstOrDefault();
            if(Depart!=null)
            {
                var DepartId = Depart.Id;
               
                if (!string.IsNullOrEmpty(DepartId))
                {
                    item = _iDepartmentService.GetById(DepartId);
                }
                ViewBag.DepartName = Depart.SysDepartment.Name;
            }
           
            ViewBag.TrainingType1 = _trainingTypeService.GetAll(a => a.Position == Position.上肢).ToList();
            ViewBag.TrainingType2 = _trainingTypeService.GetAll(a => a.Position == Position.下肢).ToList();
            ViewBag.TrainingType3 = _trainingTypeService.GetAll(a => a.Position == Position.躯干).ToList();

            return View(item);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(IEnumerable<string> TrainingTypeids)
        {
           var DepartId = _iSysDepartmentSysUserService.GetAll(a => a.SysUserId == _iUserInfo.UserId).FirstOrDefault().SysDepartmentId;


            if (!string.IsNullOrEmpty(DepartId))
            {
                //清除原有数据
                _trainingRelationService.Delete(a => a.SysDepartmentId.Equals(DepartId));
            }

            //_iSysRoleService.Save(id, collection);

            if (TrainingTypeids != null)
            {
                foreach (var item in TrainingTypeids)
                {
                    _trainingRelationService.Save(null, new TrainingRelation
                    {
                        SysDepartmentId = DepartId,
                        TrainingTypeId = item
                    });
                }
            }

            await _unitOfWork.CommitAsync();
            return View();
          

        }
    }
}