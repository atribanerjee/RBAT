using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RBAT.Core.Clasess;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RBAT.Logic;
using RBAT.Web.Extensions;
using System.IO;
using CsvHelper;
using Microsoft.Net.Http.Headers;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class TimeWaterUseController : Controller
    {
        private readonly ITimeWaterUseService _timeWaterUseService;
        private readonly INodeService _nodeService;
        private readonly ILogger _logger;

        public TimeWaterUseController(ITimeWaterUseService timeWaterUseService, INodeService nodeService, ILogger<TimeWaterUseController> logger)
        {
            _timeWaterUseService = timeWaterUseService;
            _nodeService = nodeService;
            _logger = logger;
        }

        // GET: TimeWaterUses
        public async Task<IActionResult> Index(int? nodeID)
        {
            ViewBag.title = "Water Use";
            ViewBag.controllerName = "TimeWaterUse";
            var node = await _nodeService.GetNodeByID(nodeID.GetValueOrDefault());
            ViewBag.elementName = (node != null) ? node.Name : string.Empty;
            ViewBag.elementID = nodeID;

            return View();
        }

        public IActionResult Add(int elementID, TimeComponent timeComponentType)
        {
            ViewBag.controllerName = "TimeWaterUse";
            ViewBag.elementID = elementID;
            ViewBag.timeComponentType = timeComponentType;

            return View();
        }

        // POST: TimeWaterUses/FillGrid
        public async Task<IActionResult> FillGridFromDB(int elementID, List<TimeWaterUse> importList, TimeComponent timeComponent, int start, int length, int draw)
        {
            if (length < 0)
            {
                length = 10;
            }
            try
            {
                var recordsTotal = (importList != null && importList.Any()) ? importList.Count : await _timeWaterUseService.GetTimeWaterUseTotalCount(elementID, timeComponent);
                var data = (importList != null && importList.Any()) ? importList : await _timeWaterUseService.GetTimeWaterUse(elementID, start, length, timeComponent);
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(int elementID, TimeComponent timeComponent)
        {
            try
            {
                var data = await _timeWaterUseService.GetAllTimeWaterUse(elementID, timeComponent);

                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                var csvWriter = new CsvWriter(writer);

                csvWriter.WriteField("Year");
                csvWriter.WriteField(timeComponent.ToString());
                csvWriter.WriteField("Water Use");
                csvWriter.NextRecord();

                foreach (var record in data)
                {
                    csvWriter.WriteField(record.Year);
                    csvWriter.WriteField(record.TimeComponentValue);
                    csvWriter.WriteField(record.Value);
                    csvWriter.NextRecord();
                    writer.Flush();
                }
                stream.Position = 0; //reset stream
                return new FileStreamResult(stream, new MediaTypeHeaderValue("text/csv"))
                {
                    FileDownloadName = "Water Use.csv"
                };
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // POST: TimeWaterUses/GetDateFromClipboard
        public async Task<IActionResult> GetDateFromClipboard(int elementID, DateTime startDate, TimeComponent timeComponent, List<TimeWaterUse> existingList, List<PastedTimeComponent> pasteList)
        {
            try
            {
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var combinedList = await _timeWaterUseService.GetJoinedList(elementID, startDate, timeComponent, existingList, pasteList);
                return Json(new { list = combinedList });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> SaveAll(int elementID, DateTime startDate, TimeComponent timeComponent, List<TimeWaterUse> listToSave)
        {
            try
            {
                await _timeWaterUseService.SaveAll(elementID, startDate, timeComponent, listToSave);
                return await Task.FromResult(Json(new { listToSave }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> AddNew(int elementID, TimeComponent timeComponentType, int year, int timeComponentValue, int value)
        {
            try
            {                
                var listToSave = new List<TimeWaterUse>() { new TimeWaterUse(elementID, year, (int)timeComponentType, timeComponentValue, value) };
                await _timeWaterUseService.SaveAll(listToSave);

                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(List<TimeWaterUse> listToUpdate)
        {
            try
            {
                await _timeWaterUseService.UpdateAll(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<TimeWaterUse> listToRemove)
        {
            try
            {
                await _timeWaterUseService.Remove(listToRemove);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> RemoveAll(int elementID, TimeComponent timeComponent)
        {
            try
            {
                await _timeWaterUseService.RemoveAll(elementID, timeComponent);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> OnUploadAsync(IFormFile fileSelect, int projectID, int elementID, DateTime startDate, TimeComponent timeComponent)
        {
            return await this.ReadFromExcel(fileSelect, projectID, elementID, startDate, timeComponent, _timeWaterUseService, _logger);
        }

    }
}