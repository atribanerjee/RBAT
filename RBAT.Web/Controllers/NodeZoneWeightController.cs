using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    public class NodeZoneWeightController : Controller
    {
        private readonly INodeZoneWeightService _nodeZoneWeightService;
        private readonly INodePolicyGroupService _nodePolicyGroupService;

        public NodeZoneWeightController(INodeZoneWeightService nodeZoneWeightService, INodePolicyGroupService nodePolicyGroupService)
        {
            _nodeZoneWeightService = nodeZoneWeightService;
            _nodePolicyGroupService = nodePolicyGroupService;
        }

        public async Task<IActionResult> FillGridFromDB(int nodePolicyGroupID, List<NodeZoneWeight> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var data = (importList != null && importList.Any()) ? importList : await _nodeZoneWeightService.GetAll(nodePolicyGroupID);
                int recordsTotal = data.Count;  
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data});
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> GetDateFromClipboard(int nodePolicyGroupID, List<NodeZoneWeight> existingList, List<ZoneLevelWithTimeComponent> pasteList)
        {
            try
            {                
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var combinedList = await _nodeZoneWeightService.GetJoinedList(nodePolicyGroupID, existingList, pasteList);
                return Json(new { list = combinedList });                
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }            
        }

        public async Task<IActionResult> SaveAll(int nodePolicyGroupID, List<NodeZoneWeight> listToSave)
        {
            try
            {                
                await _nodeZoneWeightService.SaveAll(nodePolicyGroupID, listToSave);
                return await Task.FromResult(Json(new { listToSave }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }       

        public async Task<IActionResult> Update(List<NodeZoneWeight> listToUpdate)
        {
            try
            {                
                await _nodeZoneWeightService.UpdateAll(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<NodeZoneWeight> listToRemove)
        {
            try
            {
                await _nodeZoneWeightService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new {  }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}