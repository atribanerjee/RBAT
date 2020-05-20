using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class ChannelOutflowCapacityController : Controller
    {
        private readonly IChannelOutflowCapacityService _ChannelOutflowCapacityService;
        private readonly IChannelService _channelService;

        public ChannelOutflowCapacityController(IChannelOutflowCapacityService ChannelOutflowCapacityService, IChannelService channelService)
        {
            _ChannelOutflowCapacityService = ChannelOutflowCapacityService;
            _channelService = channelService;        
        }

        public async Task<IActionResult> Index(int? channelID)
        {
            ViewBag.title = "Outflow Capacity";            
            var channel = await _channelService.GetChannelByID(channelID.GetValueOrDefault());
            ViewBag.elementName = (channel != null) ? channel.Name : string.Empty;
            ViewBag.elementID = channelID;
            ViewBag.firstColumnName = (channel.UpstreamChannelWithControlStructureID != null) ? "River Flow (m3/s)" : "Elevation (m)";

            return View();
        }

        public IActionResult Add(int elementID)
        {
            ViewBag.elementID = elementID;            

            return View();
        }

        public async Task<IActionResult> FillGridFromDB(int elementID, List<ChannelOutflowCapacity> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var data = (importList != null && importList.Any()) ? importList : await _ChannelOutflowCapacityService.GetAll(elementID);
                int recordsTotal = data.Count;
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> GetDateFromClipboard(List<ChannelOutflowCapacity> existingList, List<ChannelOutflowCapacity> pasteList)
        {
            try
            {
                var combinedList = await _ChannelOutflowCapacityService.GetJoinedList(existingList, pasteList);
                return Json(new { list = combinedList });                
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> SaveAll(int elementID, List<ChannelOutflowCapacity> listToSave)
        {
            try
            {
                await _ChannelOutflowCapacityService.SaveAll(elementID, listToSave);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> AddNew(int elementID, double elevation, double maximumOutflow)
        {
            try
            {                
                var listToSave = new List<ChannelOutflowCapacity>() { new ChannelOutflowCapacity(elementID, elevation, maximumOutflow) };
                await _ChannelOutflowCapacityService.SaveAll(listToSave);

                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(List<ChannelOutflowCapacity> listToUpdate)
        {
            try
            {
                await _ChannelOutflowCapacityService.UpdateAll(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<ChannelOutflowCapacity> listToRemove)
        {
            try
            {
                await _ChannelOutflowCapacityService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}