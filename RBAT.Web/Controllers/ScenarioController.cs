using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using RBAT.Web.Models.Scenario;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class ScenarioController : Controller
    {

        private readonly IScenarioService _scenarioService;

        public ScenarioController(IScenarioService scenarioService, ILogger<ScenarioController> logger)
        {
            _scenarioService = scenarioService;
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
            ViewBag.title = "Scenario List";
            ViewBag.projectID = id;
            return View();                                              
        }

        public IActionResult Add()
        {
            var model = new ScenarioModel
            {
                CalculationBeginsDate = DateTime.Now.Date,
                CalculationEndsDate = DateTime.Now.Date,
                CalculationBegins = DateTime.Now.ToString("d", CultureInfo.InvariantCulture),
                CalculationEnds = DateTime.Now.ToString("d", CultureInfo.InvariantCulture)
            };
            return View(model);
        }

        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var scenario = await _scenarioService.Get(id);
                var model = scenario.ToScenarioModel();
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
                IEnumerable<Scenario> scenarios = await _scenarioService.GetAllByProjectID(projectID.GetValueOrDefault());
                var model = scenarios.ToScenarioViewModelList();
                return Json(new { draw, recordsFiltered = model.Count(), recordsTotal = model.Count(), data = model });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> AddNew(ScenarioModel scenarioModel)
        {
            try
            {
                if (ModelState.IsValid)
                {                     
                    await _scenarioService.Save(scenarioModel.ToScenarioEntityModel());
                    return await Task.FromResult(Json(new { success = true }));
                }
                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<Scenario> listToRemove)
        {
            try
            {
                await this._scenarioService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new { Success = "True" }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }            
        }

        public async Task<IActionResult> Update(ScenarioModel scenarioModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listToUpdate = new List<Scenario> { scenarioModel.ToScenarioEntityModel() };
                    await _scenarioService.UpdateAll(listToUpdate);
                    return Json(new { success = true });
                }
                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        // GET: Scenario/CopyScenario
        [HttpGet]
        public async Task<IActionResult> CopyScenario(long scenarioId)
        {
            try
            {
                bool result = false;
                result = await this._scenarioService.CopyScenario(scenarioId);                

                return Json(new { Type = result ? "Success" : "Error" });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}
