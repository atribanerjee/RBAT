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
using System.IO;
using OfficeOpenXml;
using RBAT.Web.Extensions;
using CsvHelper;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class TimeClimateDataController : Controller
    {
        private readonly ITimeClimateDataService _timeClimateDataService;
        private readonly IClimateStationService _climateStationService;
        private readonly ILogger _logger;

        public TimeClimateDataController(ITimeClimateDataService timeClimateDataService, IClimateStationService climateStationService, ILogger<TimeClimateDataController> logger)
        {
            _timeClimateDataService = timeClimateDataService;
            _climateStationService = climateStationService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? climateStationID)
        {
            ViewBag.title = "Climate Data";
            ViewBag.controllerName = "TimeClimateData";
            var climateStation = await _climateStationService.GetClimateStationByID(climateStationID.GetValueOrDefault());
            ViewBag.elementName = (climateStation != null) ? climateStation.Name : string.Empty;
            ViewBag.elementID = climateStationID;

            return View();
        }

        public IActionResult Add(int elementID, TimeComponent timeComponentType)
        {
            ViewBag.controllerName = "TimeClimateData";
            ViewBag.elementID = elementID;
            ViewBag.timeComponentType = timeComponentType;

            return View();
        }

        public async Task<IActionResult> FillGridFromDB(int elementID, List<TimeClimateData> importList, TimeComponent timeComponent, int start, int length, int draw)
        {
            if (length < 0)
            {
                length = 10;
            }
            try
            {
                var recordsTotal = (importList != null && importList.Any()) ? importList.Count : await _timeClimateDataService.GetTimeClimateDatalTotalCount(elementID, timeComponent);
                var data = (importList != null && importList.Any()) ? importList : await _timeClimateDataService.GetTimeClimateData(elementID, start, length, timeComponent);
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
                var data = await _timeClimateDataService.GetAllTimeClimateData(elementID, timeComponent);

                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                var csvWriter = new CsvWriter(writer);

                csvWriter.WriteField("Year");
                csvWriter.WriteField(timeComponent.ToString());
                csvWriter.WriteField("Climate Data");
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
                    FileDownloadName = "Climate Data.csv"
                };
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> GetDateFromClipboard(int elementID, DateTime startDate, TimeComponent timeComponent, List<TimeClimateData> existingList, List<PastedTimeComponent> pasteList)
        {
            try
            {
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var combinedList = await _timeClimateDataService.GetJoinedList(elementID, startDate, timeComponent, existingList, pasteList);
                return Json(new { list = combinedList });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> SaveAll(int elementID, DateTime startDate, TimeComponent timeComponent, List<TimeClimateData> listToSave)
        {
            try
            {
                await _timeClimateDataService.SaveAll(elementID, startDate, timeComponent, listToSave);
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
                var listToSave = new List<TimeClimateData>() {new TimeClimateData(elementID, year, (int)timeComponentType, timeComponentValue, value)};
                await _timeClimateDataService.SaveAll(listToSave);

                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(List<TimeClimateData> listToUpdate)
        {
            try
            {
                await _timeClimateDataService.UpdateAll(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<TimeClimateData> listToRemove)
        {
            try
            {
                await _timeClimateDataService.Remove(listToRemove);
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
                await _timeClimateDataService.RemoveAll(elementID, timeComponent);
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
            return await this.ReadFromExcel(fileSelect, projectID, elementID, startDate, timeComponent, _timeClimateDataService, _logger);
        }
    }
}