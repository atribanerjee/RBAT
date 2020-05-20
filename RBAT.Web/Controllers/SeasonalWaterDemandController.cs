using Microsoft.AspNetCore.Mvc;
using RBAT.Logic.Interfaces;
using RBAT.Logic.TransferModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    public class SeasonalWaterDemandController : Controller
    {
        private readonly ISeasonalWaterDemandService _seasonalWaterDemandService;

        public SeasonalWaterDemandController(ISeasonalWaterDemandService seasonalWaterDemandService)
        {
            this._seasonalWaterDemandService = seasonalWaterDemandService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(SeasonalModel seasonalModel)
        {
            try
            {                
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var raterDemands = await this._seasonalWaterDemandService.GetAll(seasonalModel);
                int recordsTotal = raterDemands.Item2.Count();
                return Json(new { draw, recordsFiltered = raterDemands.Item2.Count(), recordsTotal, data = raterDemands });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }       
    }
}