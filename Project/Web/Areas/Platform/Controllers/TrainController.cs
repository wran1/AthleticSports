using Common;
using IServices.IDictionaryServices;
using IServices.Infrastructure;
using Models.Dictionary;
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
    public class TrainController : Controller
    {
        private readonly ITrainService _trainService;
        private readonly IUnitOfWork _unitOfWork;
        public TrainController(ITrainService trainService,IUnitOfWork unitOfWork)
        {
            _trainService = trainService;
            _unitOfWork = unitOfWork;
        }
        // GET: Platform/Train
        public ActionResult Index(string keyword, string ordering, int pageIndex = 1)
        {
            var model =
                _trainService.GetAll()
                                 .Select(
                                     a =>
                                     new
                                     {
                                         CityName = a.Name,
                                         CityFullName = a.FullName,
                                         a.SystemId,
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

        //
        // GET: /Platform/City/Details/5

        public ActionResult Details(object id)
        {
            var item = _trainService.GetById(id);
            return View(item);
        }

        public ActionResult Create()
        {
            return RedirectToAction("Edit");
        }

        //
        // GET: /Platform/City/Edit/5

        public ActionResult Edit(string id)
        {
            var item = new Train();
            if (!string.IsNullOrEmpty(id))
            {
                item = _trainService.GetById(id);
            }
            return View(item);
        }

        //
        // POST: /Platform/City/Edit/5

        [HttpPost]
        public async Task<ActionResult> Edit(string id, Train collection)
        {
            if (!ModelState.IsValid)
            {
                return View(collection);
            }

            _trainService.Save(id, collection);

            await _unitOfWork.CommitAsync();

            return new EditSuccessResult(id);
        }


        //
        // POST: /Platform/City/Delete/5

        [HttpDelete]
        public async Task<ActionResult> Delete(object id)
        {
            _trainService.Delete(id);
            await _unitOfWork.CommitAsync();
            return RedirectToAction("Index");
        }
    }
}