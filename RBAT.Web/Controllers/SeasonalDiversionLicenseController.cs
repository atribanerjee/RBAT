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
    public class SeasonalDiversionLicenseController : Controller
    {
        private readonly ISeasonalDiversionLicenseService _seasonalDiversionLicenseService;

        public SeasonalDiversionLicenseController(ISeasonalDiversionLicenseService seasonalDiversionLicenseService)
        {
            this._seasonalDiversionLicenseService = seasonalDiversionLicenseService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(SeasonalModel seasonalModel, List<SeasonalDiversionLicense> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var diversionLicenses = (importList != null && importList.Any()) ? importList : await this._seasonalDiversionLicenseService.GetAll(seasonalModel);
                int recordsTotal = diversionLicenses.Count();
                return Json(new { draw, recordsFiltered = diversionLicenses.Count(), recordsTotal, data = diversionLicenses });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetDataFromClipboard(List<SeasonalDiversionLicense> existingList, List<SeasonalDiversionLicense> pasteList)
        {
            try
            {
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var updatedList = await _seasonalDiversionLicenseService.UpdateExistingList(existingList, pasteList);
                return Json(new { list = updatedList });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}