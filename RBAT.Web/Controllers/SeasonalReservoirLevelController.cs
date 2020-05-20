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
    public class SeasonalReservoirLevelController : Controller
    {
        private readonly ISeasonalReservoirLevelService _seasonalReservoirLevelService;

        public SeasonalReservoirLevelController(ISeasonalReservoirLevelService seasonalReservoirLevelService)
        {
            this._seasonalReservoirLevelService = seasonalReservoirLevelService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(SeasonalModel seasonalModel, List<SeasonalReservoirLevel> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var reservoirLevels = (importList != null && importList.Any()) ? importList : await this._seasonalReservoirLevelService.GetAll(seasonalModel);
                int recordsTotal = reservoirLevels.Count();
                return Json(new { draw, recordsFiltered = reservoirLevels.Count(), recordsTotal, data = reservoirLevels });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetDataFromClipboard(List<SeasonalReservoirLevel> existingList, List<double> pasteList)
        {
            try
            {
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var updatedList = await _seasonalReservoirLevelService.UpdateExistingList(existingList, pasteList);
                return Json(new { list = updatedList });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}