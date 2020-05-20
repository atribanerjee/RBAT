using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using RBAT.Logic.TransferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    public class SeasonalApportionmentTargetController : Controller
    {
        private readonly ISeasonalApportionmentTargetService _seasonalApportionmentTargetService;

        public SeasonalApportionmentTargetController(ISeasonalApportionmentTargetService seasonalApportionmentTargetService)
        {
            this._seasonalApportionmentTargetService = seasonalApportionmentTargetService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(SeasonalModel seasonalModel, List<SeasonalApportionmentTarget> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var apportionmentTargets = (importList != null && importList.Any()) ? importList : await this._seasonalApportionmentTargetService.GetAll(seasonalModel);
                int recordsTotal = apportionmentTargets.Count();
                return Json(new { draw, recordsFiltered = apportionmentTargets.Count(), recordsTotal, data = apportionmentTargets });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetDataFromClipboard(List<SeasonalApportionmentTarget> existingList, List<double> pasteList)
        {
            try
            {
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var updatedList = await _seasonalApportionmentTargetService.UpdateExistingList(existingList, pasteList);
                return Json(new { list = updatedList });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}