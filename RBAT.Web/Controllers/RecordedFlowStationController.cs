using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using RBAT.Web.Models.RecordedFlowStation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class RecordedFlowStationController : Controller {

        private readonly IRecordedFlowStationService _recordedFlowStationService;

        public RecordedFlowStationController( IRecordedFlowStationService recordedFlowStationService) {
            _recordedFlowStationService = recordedFlowStationService;
        }

        public JsonResult ModelStateErrors
        {
            get
            {
                var errorModel =
                    from x in ModelState.Keys
                    where ModelState[x].Errors.Count > 0
                    select new
                    {
                        key = x,
                        errors = ModelState[x].Errors.Select(y => y.ErrorMessage).ToArray()
                    };
                return Json(new { success = false, errors = errorModel });
            }
        }

        public IActionResult Index() {
            ViewBag.isModal = false;
            ViewBag.title = "Recorded Flow Station List";
            return View();
        }

        public IActionResult Add() {
            return View(new RecordedFlowStationModel());
        }        

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var recordedFlowStation = await _recordedFlowStationService.GetRecordedFlowStationByID(id);
                return View(recordedFlowStation.ToRecordedFlowStationModel());
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }        

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                IEnumerable<RecordedFlowStation> recordedFlowStations = await _recordedFlowStationService.GetAll();
                var model = recordedFlowStations.ToRecordedFlowStationViewModelList();
                return Json(new { draw, recordsFiltered = model.Count(), recordsTotal = model.Count(), data = model });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> AddNew(RecordedFlowStationModel recordedFlowStationModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _recordedFlowStationService.Save(recordedFlowStationModel.ToRecordedFlowStationEntityModel());
                    return await Task.FromResult(Json(new { success = true }));
                }
                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(RecordedFlowStationModel recordedFlowStationModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listToUpdate = new List<RecordedFlowStation> { recordedFlowStationModel.ToRecordedFlowStationEntityModel() };
                    await _recordedFlowStationService.UpdateAll(listToUpdate);
                    return Json(new { success = true });
                }

                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<RecordedFlowStation> listToRemove)
        {
            try
            {
                await _recordedFlowStationService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new { Success = "True" }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}
