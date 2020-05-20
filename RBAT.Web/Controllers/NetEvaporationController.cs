using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic;
using RBAT.Logic.Interfaces;
using RBAT.Logic.TransferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RBAT.Web.Controllers {

    [Authorize]
    public class NetEvaporationController : Controller {

        private readonly INetEvaporationService _netEvaporationService;
        private readonly INodeService _nodeService;

        public NetEvaporationController(INetEvaporationService netEvaporationService, INodeService nodeService) {
            _netEvaporationService = netEvaporationService;
            _nodeService = nodeService;
        }

        public async Task<IActionResult> Index(int? nodeID)
        {
            ViewBag.title = "Net Evaporation";            
            var node = await _nodeService.GetNodeByID(nodeID.GetValueOrDefault());
            ViewBag.elementName = (node != null) ? node.Name : string.Empty;
            ViewBag.nodeID = nodeID;

            return View();
        }

        public IActionResult Add(int nodeID, int climateStationID)
        {
            ViewBag.nodeID = nodeID;
            ViewBag.climateStationID = climateStationID;

            return View();
        }

        public IActionResult Edit(int id, int climateStationID, double adjustmentFactor)
        {            
            ViewBag.id = id; 
            ViewBag.climateStationID = climateStationID;
            ViewBag.adjustmentFactor = adjustmentFactor;

            return View();
        }

        public async Task<IActionResult> GetAllData(int nodeID)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var data =  await _netEvaporationService.GetAll(nodeID);
                int recordsTotal = data.Count;
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }       
       
        public async Task<IActionResult> AddNew(int nodeID, int climateStationID, double adjustmentFactor)
        {
            try
            {
                var newItem = new NetEvaporation(nodeID, climateStationID, adjustmentFactor);
                var listToSave = new List<NetEvaporation> { newItem };
                await _netEvaporationService.SaveAll(listToSave);

                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(int id, int climateStationID, double adjustmentFactor)
        {
            try
            {
                var item = new NetEvaporation
                {
                    Id = id,
                    ClimateStationID = climateStationID,
                    AdjustmentFactor = adjustmentFactor
                };
                await _netEvaporationService.Update(item);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<NetEvaporation> listToRemove)
        {
            try
            {
                await _netEvaporationService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}
