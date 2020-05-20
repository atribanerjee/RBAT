using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{

    [Authorize]
    public class CustomTimeStepController : Controller {
        
        private readonly ICustomTimeStepService _customTimeStepService;
        private readonly IScenarioService _scenarioService;

        public CustomTimeStepController(ICustomTimeStepService customTimeStepService, IScenarioService scenarioService) {
            this._customTimeStepService = customTimeStepService;
            this._scenarioService = scenarioService;
        }

        /// <summary>
        /// Returns the model state errors as JSON.
        /// </summary>
        public JsonResult ModelStateErrors {
            get {
                var errorModel =
                    from x in ModelState.Keys
                    where ModelState[x].Errors.Count > 0
                    select new {
                        key = x,
                        errors = ModelState[x].Errors.Select( y => y.ErrorMessage ).ToArray()
                    };
                return Json( new { success = false, errors = errorModel } );
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(long? scenarioId) {
            ViewBag.title = "Time Steps";
            var scenario = await this._scenarioService.Get(scenarioId.GetValueOrDefault());
            ViewBag.elementName = (scenario != null) ? scenario.Name : string.Empty;
            ViewBag.elementID = scenarioId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FillCustomTimeStep(long scenarioId, List<CustomTimeStep> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var data = (importList != null && importList.Any()) ? importList : await this._customTimeStepService.GetCustomTimeSteps(scenarioId);
                int recordsTotal = data.Count();
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }


        public async Task<IActionResult> SaveCustomTimeSteps(int elementID, List<CustomTimeStep> listToSave)
        {
            try
            {
                await this._customTimeStepService.SaveProjectCustomTimeSteps(elementID, listToSave);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> GetDateFromClipboard(List<CustomTimeStep> existingList, List<CustomTimeStep> pasteList)
        {
            try
            {
                var combinedList = await this._customTimeStepService.GetJoinedList(existingList, pasteList);
                return Json(new { list = combinedList });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> UpdateCustomTimeStep(List<CustomTimeStep> listToUpdate)
        {
            try
            {
                await this._customTimeStepService.UpdateCustomTimeStep(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> RemoveCustomTimeStep(List<CustomTimeStep> listToRemove)
        {
            try
            {
                await this._customTimeStepService.RemoveCustomTimeStep(listToRemove);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> AddCustomTimeStep(long scenarioId, decimal newStepValue)
        {
            try
            {
                await this._customTimeStepService.SaveCustomTimeStep(scenarioId, newStepValue);
                return await Task.FromResult(Json(new { success = true }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }

}
