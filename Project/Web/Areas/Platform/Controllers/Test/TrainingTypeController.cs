using Common;
using IServices.Infrastructure;
using IServices.ITestServices;
using Models.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Platform.Helpers;
using Web.Helpers;

namespace Web.Areas.Platform.Controllers
{
    public class TrainingTypeController : Controller
    {
        // GET: Platform/TrainingType
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITrainingTypeService _trainingTypeService;
        public TrainingTypeController(IUnitOfWork unitOfWork, ITrainingTypeService trainingTypeService)
        {
            _unitOfWork = unitOfWork;
            _trainingTypeService = trainingTypeService;
        }
        public ActionResult Index(string keyword, string ordering, int pageIndex = 1)
        {
            var model =
                _trainingTypeService.GetAll()
                                 .Select(
                                     a =>
                                     new
                                     {
                                         a.Name,
                                         a.Position,
                                         a.CreatedDate,
                                         a.Remark,
                                         a.Id
                                     }).Search(keyword);

            if (!string.IsNullOrEmpty(ordering))
            {
                model = model.OrderBy(ordering, null);
            }
            return View(model.ToPagedList(pageIndex));
        }
        public ActionResult Details(object id)
        {
            var item = _trainingTypeService.GetById(id);
            return View(item);
        }

        public ActionResult Create(string updatedId)
        {
            return RedirectToAction("Edit", new { id = "", updatedId });
        }

        public ActionResult Edit(string id)
        {
            var item = new TrainingType();
            if (!string.IsNullOrEmpty(id))
            {
                item = _trainingTypeService.GetById(id);
            }
            return View(item);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, TrainingType collection)
        {
          
            if (!ModelState.IsValid)
            {
                Edit(id);
                return View(collection);
            }
            _trainingTypeService.Save(id, collection);

            await _unitOfWork.CommitAsync();

            return new EditSuccessResult(id);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(object id)
        {
            _trainingTypeService.Delete(id);
            await _unitOfWork.CommitAsync();
            return new DeleteSuccessResult();
        }
    }
}