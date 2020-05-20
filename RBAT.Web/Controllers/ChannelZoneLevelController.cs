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
    public class ChannelZoneLevelController : Controller
    {
        private readonly IChannelZoneLevelService _channelZoneLevelService;

        public ChannelZoneLevelController(IChannelZoneLevelService channelZoneLevelService)
        {
            _channelZoneLevelService = channelZoneLevelService;
        }       

        public async Task<IActionResult> FillGridFromDB(int channelPolicyGroupID, int channelID, List<ChannelZoneLevel> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var data = (importList != null && importList.Any()) ? importList : await _channelZoneLevelService.GetAll(channelPolicyGroupID, channelID);
                int recordsTotal = data.Count;  
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data});
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> GetDateFromClipboard(int channelPolicyGroupID, int channelID, List<ChannelZoneLevel> existingList, List<ZoneLevelWithTimeComponent> pasteList)
        {
            try
            {                
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var combinedList = await _channelZoneLevelService.GetJoinedList(channelPolicyGroupID, channelID, existingList, pasteList);
                return Json(new { list = combinedList });                
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }            
        }

        public async Task<IActionResult> SaveAll(int channelPolicyGroupID, int channelID, List<ChannelZoneLevel> listToSave)
        {
            try
            {                
                await _channelZoneLevelService.SaveAll(channelPolicyGroupID, channelID, listToSave);
                return await Task.FromResult(Json(new { listToSave }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(List<ChannelZoneLevel> listToUpdate)
        {
            try
            {                
                await _channelZoneLevelService.UpdateAll(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<ChannelZoneLevel> listToRemove)
        {
            try
            {
                await _channelZoneLevelService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new {  }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}