using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using RBAT.Web.Models.TimeStorageCapacity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class TimeStorageCapacityController : Controller
    {
        private readonly ITimeStorageCapacityService _timeStorageCapacityService;
        private readonly INodeService _nodeService;

        public TimeStorageCapacityController(ITimeStorageCapacityService timeStorageCapacityService, INodeService nodeService)
        {
            _timeStorageCapacityService = timeStorageCapacityService;
            _nodeService = nodeService;        
        }

        public async Task<IActionResult> Index(int? nodeID)
        {
            ViewBag.title = "Storage Capacity";
            var node = await _nodeService.GetNodeByID(nodeID.GetValueOrDefault());
            ViewBag.elementName = (node != null) ? node.Name : string.Empty;
            ViewBag.elementID = nodeID;

            return View();
        }

        public IActionResult Add(int elementID)
        {
            ViewBag.elementID = elementID;            

            return View();
        }

        public async Task<IActionResult> FillGridFromDB(int elementID, List<TimeStorageCapacity> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var data = (importList != null && importList.Any()) ? importList : await _timeStorageCapacityService.GetAll(elementID);
                int recordsTotal = data.Count;
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = data.ToTimeStorageCapacityViewModelList() });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> GetDateFromClipboard(List<TimeStorageCapacity> existingList, List<TimeStorageCapacity> pasteList)
        {
            try
            {
                var combinedList = await _timeStorageCapacityService.GetJoinedList(existingList, pasteList);
                return Json(new { list = combinedList.ToTimeStorageCapacityViewModelList() });                
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> SaveAll(int elementID, List<TimeStorageCapacity> listToSave)
        {
            try
            {
                await _timeStorageCapacityService.SaveAll(elementID, listToSave);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> AddNew(int elementID, DateTime surveyDate, double elevation, double area, double volume)
        {
            try
            {                
                var listToSave = new List<TimeStorageCapacity>() { new TimeStorageCapacity(elementID, surveyDate, elevation, area, volume) };
                await _timeStorageCapacityService.SaveAll(listToSave);

                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(List<TimeStorageCapacity> listToUpdate)
        {
            try
            {
                await _timeStorageCapacityService.UpdateAll(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<TimeStorageCapacity> listToRemove)
        {
            try
            {
                await _timeStorageCapacityService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}