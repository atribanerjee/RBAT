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
    public class ChannelTravelTimeController : Controller
    {
        private readonly IChannelTravelTimeService _ChannelTravelTimeService;
        private readonly IChannelService _channelService;

        public ChannelTravelTimeController(IChannelTravelTimeService ChannelTravelTimeService, IChannelService channelService)
        {
            _ChannelTravelTimeService = ChannelTravelTimeService;
            _channelService = channelService;        
        }

        public async Task<IActionResult> Index(int? channelID)
        {
            ViewBag.title = "Travel Time";
            var channel = await _channelService.GetChannelByID(channelID.GetValueOrDefault());
            ViewBag.elementName = (channel != null) ? channel.Name : string.Empty;
            ViewBag.elementID = channelID;

            return View();
        }

        public IActionResult Add(int elementID)
        {
            ViewBag.elementID = elementID;            

            return View();
        }

        public async Task<IActionResult> FillGridFromDB(int elementID, List<ChannelTravelTime> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var data = (importList != null && importList.Any()) ? importList : await _ChannelTravelTimeService.GetAll(elementID);
                int recordsTotal = data.Count;
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> GetDateFromClipboard(List<ChannelTravelTime> existingList, List<ChannelTravelTime> pasteList)
        {
            try
            {
                var combinedList = await _ChannelTravelTimeService.GetJoinedList(existingList, pasteList);
                return Json(new { list = combinedList });                
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> SaveAll(int elementID, List<ChannelTravelTime> listToSave)
        {
            try
            {
                await _ChannelTravelTimeService.SaveAll(elementID, listToSave);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> AddNew(int elementID, double flow, double travelTime)
        {
            try
            {                
                var listToSave = new List<ChannelTravelTime>() { new ChannelTravelTime(elementID, flow, travelTime) };
                await _ChannelTravelTimeService.SaveAll(listToSave);

                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(List<ChannelTravelTime> listToUpdate)
        {
            try
            {
                await _ChannelTravelTimeService.UpdateAll(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<ChannelTravelTime> listToRemove)
        {
            try
            {
                await _ChannelTravelTimeService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}