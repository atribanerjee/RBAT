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
    public class TimeNaturalFlowController : Controller
    {
        private readonly ITimeNaturalFlowService _timeNaturalFlowService;
        private readonly INodeService _nodeService;
        private readonly ILogger _logger;

        public TimeNaturalFlowController(ITimeNaturalFlowService timeNaturalFlowService, INodeService nodeService, ILogger<TimeNaturalFlowController> logger)
        {
            _timeNaturalFlowService = timeNaturalFlowService;
            _nodeService = nodeService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? nodeID)
        {
            ViewBag.title = "Natural Flow";
            ViewBag.controllerName = "TimeNaturalFlow";
            var node = await _nodeService.GetNodeByID(nodeID.GetValueOrDefault());
            ViewBag.elementName = (node != null) ? node.Name : string.Empty;
            ViewBag.elementID = nodeID;            
            
            return View();
        }

        public IActionResult Add(int projectID, int elementID, TimeComponent timeComponentType)
        {
            ViewBag.controllerName = "TimeNaturalFlow";
            ViewBag.projectID = projectID;
            ViewBag.elementID = elementID;
            ViewBag.timeComponentType = timeComponentType;      
            
            return View();
        }

        public async Task<IActionResult> FillGridFromDB(int projectID, int elementID, List<TimeNaturalFlow> importList, TimeComponent timeComponent, int start, int length, int draw)
        {
            if (length < 0)
            {
                length = 10;
            }
            try
            {
                var recordsTotal = (importList != null && importList.Any()) ? importList.Count : await _timeNaturalFlowService.GetTimeNaturalFlowsTotalCount(projectID, elementID, timeComponent);
                var data = (importList != null && importList.Any()) ? importList : await _timeNaturalFlowService.GetTimeNaturalFlows(projectID, elementID, start, length, timeComponent);
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data});
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetAll(int projectID, int elementID, TimeComponent timeComponent)
        {
            try
            {
                var data = await _timeNaturalFlowService.GetAllTimeNaturalFlows(elementID, timeComponent, projectID);

                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                var csvWriter = new CsvWriter(writer);
                 
                csvWriter.WriteField("Year");
                csvWriter.WriteField(timeComponent.ToString());
                csvWriter.WriteField("Natural Flow");
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
                    FileDownloadName = "Natural Flow.csv"
                };
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> GetDateFromClipboard(int projectID, int elementID, DateTime startDate, TimeComponent timeComponent, List<TimeNaturalFlow> existingList, List<PastedTimeComponent> pasteList)
        {
            try
            {
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                var combinedList = await _timeNaturalFlowService.GetJoinedList(projectID, elementID, startDate, timeComponent, existingList, pasteList);
                return Json(new { list = combinedList });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }            
        }

        public async Task<IActionResult> SaveAll(int projectID, int elementID, DateTime startDate, TimeComponent timeComponent, List<TimeNaturalFlow> listToSave)
        {
            try
            {
                await _timeNaturalFlowService.SaveAll(projectID, elementID, startDate, timeComponent, listToSave);
                return await Task.FromResult(Json(new { listToSave }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> AddNew(int projectID, int elementID, TimeComponent timeComponentType, int year, int timeComponentValue, int value)
        {
            try
            {
                var newItem = new TimeNaturalFlow(projectID, elementID, year, (int)timeComponentType, timeComponentValue, value);
                var listToSave = new List<TimeNaturalFlow> { newItem };
                await _timeNaturalFlowService.SaveAll(listToSave);

                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(List<TimeNaturalFlow> listToUpdate)
        {
            try
            {                
                await _timeNaturalFlowService.UpdateAll(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<TimeNaturalFlow> listToRemove)
        {
            try
            {
                await _timeNaturalFlowService.Remove(listToRemove);
                return await Task.FromResult(Json(new {  }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> RemoveAll(int projectID, int elementID, TimeComponent timeComponent)
        {
            try
            {
                await _timeNaturalFlowService.RemoveAll(elementID, timeComponent, projectID);
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
            return await this.ReadFromExcel(fileSelect, projectID, elementID, startDate, timeComponent, _timeNaturalFlowService, _logger);
        }
    }   
}