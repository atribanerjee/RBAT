using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using RBAT.Web.Models.StartingReservoirLevel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class StartingReservoirLevelController : Controller
    {

        private readonly IStartingReservoirLevelService _startingReservoirLevelService;

        public StartingReservoirLevelController(IStartingReservoirLevelService startingReservoirLevelService, ILogger<StartingReservoirLevelController> logger)
        {
            _startingReservoirLevelService = startingReservoirLevelService;
        }

        /// <summary>
        /// Returns the model state errors as JSON.
        /// </summary>
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

        public IActionResult Index(int? id)
        {
            ViewBag.title = "Starting Reservoir Level List";
            ViewBag.projectID = id;
            return View();                                              
        }

        public IActionResult Add(int projectID)
        {
            var model = new StartingReservoirLevelModel
            {
                ProjectID = projectID
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var startingReservoirLevel = await _startingReservoirLevelService.Get(id);
                var model = startingReservoirLevel.ToStartingReservoirLevelModel();
                return View(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> GetAll(int? projectID)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                IEnumerable<StartingReservoirLevel> startingReservoirLevels = await _startingReservoirLevelService.GetAllByProjectID(projectID.GetValueOrDefault());
                var model = startingReservoirLevels.ToStartingReservoirLevelViewModelList();
                return Json(new { draw, recordsFiltered = model.Count(), recordsTotal = model.Count(), data = model });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> AddNew(StartingReservoirLevelModel startingReservoirLevelModel)
        {
            try
            {
                if (ModelState.IsValid)
                {                     
                    await _startingReservoirLevelService.Save(startingReservoirLevelModel.ToStartingReservoirLevelEntityModel());
                    return await Task.FromResult(Json(new { success = true }));
                }
                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<StartingReservoirLevel> listToRemove)
        {
            try
            {
                await this._startingReservoirLevelService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new { Success = "True" }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(StartingReservoirLevelModel startingReservoirLevelModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listToUpdate = new List<StartingReservoirLevel> { startingReservoirLevelModel.ToStartingReservoirLevelEntityModel() };
                    await _startingReservoirLevelService.UpdateAll(listToUpdate);
                    return Json(new { success = true });
                }
                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}
