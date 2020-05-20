using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using RBAT.Core.Clasess;
using RBAT.Core.Models;
using RBAT.Logic;
using RBAT.Logic.Interfaces;
using RBAT.Web.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class TimeRecordedFlowController : Controller
    {
        private readonly ITimeRecordedFlowService _timeRecordedFlowService;
        private readonly IRecordedFlowStationService _recordedFlowStationService;
        private readonly ILogger _logger;

        public TimeRecordedFlowController(ITimeRecordedFlowService timeRecordedFlowService, IRecordedFlowStationService recordedFlowStationService, ILogger<TimeRecordedFlowController> logger)
        {
            _timeRecordedFlowService = timeRecordedFlowService;
            _recordedFlowStationService = recordedFlowStationService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? recordedFlowStationID)
        {
            ViewBag.title = "Recorded Flow";
            ViewBag.controllerName = "TimeRecordedFlow";
            var recordedflowStation = await _recordedFlowStationService.GetRecordedFlowStationByID(recordedFlowStationID.GetValueOrDefault());
            ViewBag.elementName = (recordedflowStation != null) ? recordedflowStation.Name : string.Empty;
            ViewBag.elementID = recordedFlowStationID;            
            
            return View();
        }

        public IActionResult Add(int elementID, TimeComponent timeComponentType)
        {
            ViewBag.controllerName = "TimeRecordedFlow";
            ViewBag.elementID = elementID;
            ViewBag.timeComponentType = timeComponentType;      
            
            return View();
        }

        public async Task<IActionResult> FillGridFromDB(int elementID, List<TimeRecordedFlow> importList, TimeComponent timeComponent, int start, int length, int draw)
        {
            if (length < 0)
            {
                length = 10;
            }
            try
            {
                var recordsTotal = (importList != null && importList.Any()) ? importList.Count : await _timeRecordedFlowService.GetTimeRecordedFlowsTotalCount(elementID, timeComponent);
                var data = (importList != null && importList.Any()) ? importList : await _timeRecordedFlowService.GetTimeRecordedFlows(elementID, start, length, timeComponent);
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data});
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
                var data = await _timeRecordedFlowService.GetAllTimeRecordedFlows(elementID, timeComponent);

                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                var csvWriter = new CsvWriter(writer);

                csvWriter.WriteField("Year");
                csvWriter.WriteField(timeComponent.ToString());
                csvWriter.WriteField("Recorded Flow");
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
                    FileDownloadName = "Recorded Flow.csv"
                };
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> GetDateFromClipboard(int elementID, DateTime startDate, TimeComponent timeComponent, List<TimeRecordedFlow> existingList, List<PastedTimeComponent> pasteList)
        {
            try
            {
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var combinedList = await _timeRecordedFlowService.GetJoinedList(elementID, startDate, timeComponent, existingList, pasteList);
                return Json(new { list = combinedList });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }            
        }

        public async Task<IActionResult> SaveAll(int elementID, DateTime startDate, TimeComponent timeComponent, List<TimeRecordedFlow> listToSave)
        {
            try
            {
                await _timeRecordedFlowService.SaveAll(elementID, startDate, timeComponent, listToSave);
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
                var newItem = new TimeRecordedFlow(elementID, year, (int)timeComponentType, timeComponentValue, value);
                var listToSave = new List<TimeRecordedFlow> { newItem };
                await _timeRecordedFlowService.SaveAll(listToSave);

                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(List<TimeRecordedFlow> listToUpdate)
        {
            try
            {                
                await _timeRecordedFlowService.UpdateAll(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<TimeRecordedFlow> listToRemove)
        {
            try
            {
                await _timeRecordedFlowService.Remove(listToRemove);
                return await Task.FromResult(Json(new {  }));
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
                await _timeRecordedFlowService.RemoveAll(elementID, timeComponent);
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
            return await this.ReadFromExcel(fileSelect, projectID, elementID, startDate, timeComponent, _timeRecordedFlowService, _logger);
        }
    }
}