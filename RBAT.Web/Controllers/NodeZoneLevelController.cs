using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Clasess;
using RBAT.Core.Models;
using RBAT.Logic;
using RBAT.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    public class NodeZoneLevelController : Controller
    {
        private readonly INodeZoneLevelService _nodeZoneLevelService;

        public NodeZoneLevelController(INodeZoneLevelService nodeZoneLevelService)
        {
            _nodeZoneLevelService = nodeZoneLevelService;
        }       

        public async Task<IActionResult> FillGridFromDB(int nodePolicyGroupID, int nodeID, List<NodeZoneLevel> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var data = (importList != null && importList.Any()) ? importList : await _nodeZoneLevelService.GetAll(nodePolicyGroupID, nodeID);
                int recordsTotal = data.Count;  
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data});
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> GetDateFromClipboard(int nodePolicyGroupID, int nodeID, List<NodeZoneLevel> existingList, List<ZoneLevelWithTimeComponent> pasteList)
        {
            try
            {                
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var combinedList = await _nodeZoneLevelService.GetJoinedList(nodePolicyGroupID, nodeID, existingList, pasteList);
                return Json(new { list = combinedList });                
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }            
        }

        public async Task<IActionResult> SaveAll(int nodePolicyGroupID, int nodeID, List<NodeZoneLevel> listToSave)
        {
            try
            {                
                await _nodeZoneLevelService.SaveAll(nodePolicyGroupID, nodeID, listToSave);
                return await Task.FromResult(Json(new { listToSave }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }        

        public async Task<IActionResult> Update(List<NodeZoneLevel> listToUpdate)
        {
            try
            {                
                await _nodeZoneLevelService.UpdateAll(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<NodeZoneLevel> listToRemove)
        {
            try
            {
                await _nodeZoneLevelService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new {  }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}