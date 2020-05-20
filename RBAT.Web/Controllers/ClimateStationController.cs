using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using RBAT.Web.Models.ClimateStation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    [Authorize]

    public class ClimateStationController : Controller
    {

        private readonly IClimateStationService _climateStationService;
        private readonly ILogger<ClimateStationController> _logger;

        public ClimateStationController(IClimateStationService climateStationService, ILogger<ClimateStationController> logger)
        {
            _climateStationService = climateStationService;
            _logger = logger;
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

        public IActionResult Index()
        {
            ViewBag.title = "Climate Stations List";
            return View();
        }

        public IActionResult Add()
        {
            return View(new ClimateStationModel());
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var climateStation = await _climateStationService.GetClimateStationByID(id);                
                return View(climateStation.ToClimateStationModel());
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
                IEnumerable<ClimateStation> climateStations = await _climateStationService.GetAll();
                var model = climateStations.ToClimateStationViewModelList();
                return Json(new { draw, recordsFiltered = model.Count(), recordsTotal = model.Count(), data = model });

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
   
        public async Task<IActionResult> AddNew(ClimateStationModel climateStationModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _climateStationService.Save(climateStationModel.ToClimateStationEntityModel());
                    return await Task.FromResult(Json(new { success = true }));
                }
                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(ClimateStationModel climateStationModel)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    var listToUpdate = new List<ClimateStation> { climateStationModel.ToClimateStationEntityModel() };
                    await _climateStationService.UpdateAll(listToUpdate);
                    return Json(new { success = true });
                }

                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<ClimateStation> listToRemove)
        {
            try
            {
                await _climateStationService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new { Success = "True" }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}
