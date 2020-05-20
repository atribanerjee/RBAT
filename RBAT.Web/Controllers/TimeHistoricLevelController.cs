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

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class TimeHistoricLevelController : Controller
    {
        private readonly ITimeHistoricLevelService _timeHistoricLevelService;
        private readonly INodeService _nodeService;
        private readonly ILogger _logger;

        public TimeHistoricLevelController(ITimeHistoricLevelService timeHistoricLevelService, INodeService nodeService, ILogger<TimeHistoricLevelController> logger)
        {
            _timeHistoricLevelService = timeHistoricLevelService;
            _nodeService = nodeService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? nodeID)
        {
            ViewBag.title = "Historic Levels";
            ViewBag.controllerName = "TimeHistoricLevel";
            var node = await _nodeService.GetNodeByID(nodeID.GetValueOrDefault());
            ViewBag.elementName = (node != null) ? node.Name : string.Empty;
            ViewBag.elementID = nodeID;

            return View();
        }

        public IActionResult Add(int projectID, int elementID, TimeComponent timeComponentType)
        {
            ViewBag.controllerName = "TimeHistoricLevel";
            ViewBag.projectID = projectID;
            ViewBag.elementID = elementID;
            ViewBag.timeComponentType = timeComponentType;

            return View();
        }

        public async Task<IActionResult> FillGridFromDB(int elementID, List<TimeHistoricLevel> importList, TimeComponent timeComponent, int start, int length, int draw)
        {
            if (length < 0)
            {
                length = 10;
            }
            try
            {
                var recordsTotal = (importList != null && importList.Any()) ? importList.Count : await _timeHistoricLevelService.GetTimeHistoricLevelTotalCount(elementID, timeComponent);
                var data = (importList != null && importList.Any()) ? importList : await _timeHistoricLevelService.GetTimeHistoricLevel(elementID, start, length, timeComponent);
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
                var data = await _timeHistoricLevelService.GetAllTimeHistoricLevels(elementID, timeComponent);

                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                var csvWriter = new CsvWriter(writer);
                
                csvWriter.WriteField("Year");
                csvWriter.WriteField(timeComponent.ToString());
                csvWriter.WriteField("Historic Level");
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
                    FileDownloadName = "Historic Level.csv"
                };
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> GetDateFromClipboard(int elementID, DateTime startDate, TimeComponent timeComponent, List<TimeHistoricLevel> existingList, List<PastedTimeComponent> pasteList)
        {
            try
            {
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var combinedList = await _timeHistoricLevelService.GetJoinedList(elementID, startDate, timeComponent, existingList, pasteList);
                return Json(new { list = combinedList });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> SaveAll(int elementID, DateTime startDate, TimeComponent timeComponent, List<TimeHistoricLevel> listToSave)
        {
            try
            {
                await _timeHistoricLevelService.SaveAll(elementID, startDate, timeComponent, listToSave);
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
                var listToSave = new List<TimeHistoricLevel>() { new TimeHistoricLevel(elementID, year, (int)timeComponentType, timeComponentValue, value) };
                await _timeHistoricLevelService.SaveAll(listToSave);

                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(List<TimeHistoricLevel> listToUpdate)
        {
            try
            {
                await _timeHistoricLevelService.UpdateAll(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<TimeHistoricLevel> listToRemove)
        {
            try
            {
                await _timeHistoricLevelService.Remove(listToRemove);
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
                await _timeHistoricLevelService.RemoveAll(elementID, timeComponent);
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
            return await this.ReadFromExcel(fileSelect, projectID, elementID, startDate, timeComponent, _timeHistoricLevelService, _logger);
        }
    }
}