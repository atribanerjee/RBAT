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
    public class ChannelZoneWeightController : Controller
    {
        private readonly IChannelZoneWeightService _channelZoneWeightService;
        private readonly IChannelPolicyGroupService _channelPolicyGroupService;

        public ChannelZoneWeightController(IChannelZoneWeightService channelZoneWeightService, IChannelPolicyGroupService channelPolicyGroupService)
        {
            _channelZoneWeightService = channelZoneWeightService;
            _channelPolicyGroupService = channelPolicyGroupService;
        }

        public async Task<IActionResult> FillGridFromDB(int channelPolicyGroupID, List<ChannelZoneWeight> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var data = (importList != null && importList.Any()) ? importList : await _channelZoneWeightService.GetAll(channelPolicyGroupID);
                int recordsTotal = data.Count;  
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data});
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> GetDateFromClipboard(int channelPolicyGroupID, List<ChannelZoneWeight> existingList, List<ZoneLevelWithTimeComponent> pasteList)
        {
            try
            {                
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var combinedList = await _channelZoneWeightService.GetJoinedList(channelPolicyGroupID, existingList, pasteList);
                return Json(new { list = combinedList });                
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }            
        }

        public async Task<IActionResult> SaveAll(int channelPolicyGroupID, List<ChannelZoneWeight> listToSave)
        {
            try
            {                
                await _channelZoneWeightService.SaveAll(channelPolicyGroupID, listToSave);
                return await Task.FromResult(Json(new { listToSave }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }       

        public async Task<IActionResult> Update(List<ChannelZoneWeight> listToUpdate)
        {
            try
            {                
                await _channelZoneWeightService.UpdateAll(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<ChannelZoneWeight> listToRemove)
        {
            try
            {
                await _channelZoneWeightService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new {  }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}