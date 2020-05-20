using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAT.Logic;
using RBAT.Logic.Interfaces;
using RBAT.Logic.TransferModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class SolverController : Controller
    {
        const string contentType = "application/zip";
        const string attachmentFile = "Attachments.zip";
        private readonly ISolverService _solverService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly DropDownService _dropDownService;

        public SolverController(ISolverService solverService, IHttpContextAccessor contextAccessor, DropDownService dropDownService)
        {
            this._solverService = solverService;
            this._contextAccessor = contextAccessor;
            this._dropDownService = dropDownService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult ScenarioList(int projectID)
        {
            var scenarioList = this._dropDownService.ListScenarios(projectID);
            return Json(new { scenarioList });
        }       

        [HttpPost]
        public async Task<IActionResult> RunSolver(int projectId, int scenarioId, bool isSeasonalModel, bool saveConstraintsAsTXTFile, 
                                                   bool saveConstraintsAsLINDOFile, bool saveOptimalSolutionsAsTXTFile, bool saveResultsAsVolumes, 
                                                   bool isDebugMode, bool saveComponentName, double sensitivityAnalysis, double aridityFactor, SeasonalModel seasonalModel)
        {
            try
            {
                List<FileInfo> listFiles = await _solverService.RunSolver(projectId, scenarioId, isSeasonalModel, saveConstraintsAsTXTFile,
                                                                          saveConstraintsAsLINDOFile, saveOptimalSolutionsAsTXTFile, saveResultsAsVolumes, isDebugMode, saveComponentName, sensitivityAnalysis, aridityFactor, seasonalModel);

                if (listFiles == null || !listFiles.Any())
                    return Content("filename not present");

                var optimalSolutionsPath = GetOptimalSolutionsPath();

                using (var fileStream = new FileStream(optimalSolutionsPath + attachmentFile, FileMode.Create))
                {
                    using (var ziparchive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                    {
                        foreach (FileInfo fileInfo in listFiles)
                        {
                            if (!IsFileLocked(fileInfo))
                            {
                                ziparchive.CreateEntryFromFile(fileInfo.FullName, fileInfo.Name);
                            }
                        }
                    }

                    fileStream.Position = 0;
                    File(fileStream, contentType, attachmentFile);

                    foreach (FileInfo file in listFiles)
                    {
                        if (!IsFileLocked(file))
                        {
                            System.IO.File.Delete(file.FullName);
                        }
                    }
                }                

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> RunSolverFromWebService(int projectId, long scenarioId, bool saveConstraintsAsTXTFile,
                                                    bool saveConstraintsAsLINDOFile, bool saveOptimalSolutionsAsTXTFile, bool saveResultsAsVolumes,
                                                    bool isDebugMode, bool saveComponentName, double sensitivityAnalysis, double aridityFactor)
        {
            try
            {
                var content = await this._solverService.RunSolverFromWebService(projectId, scenarioId, saveConstraintsAsTXTFile,
                                                     saveConstraintsAsLINDOFile, saveOptimalSolutionsAsTXTFile, saveResultsAsVolumes,
                                                     isDebugMode, saveComponentName, sensitivityAnalysis, aridityFactor);

                var fileName = "AttachmentsFromWebService-" + Convert.ToString(DateTime.Now);
                return File(content, "application/zip", fileName + ".zip");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Message = ex.Message });
            }
        }


        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        [HttpGet]
        public IActionResult DownloadZip()
        {
            var optimalSolutionsPath = GetOptimalSolutionsPath();
            HttpContext.Response.ContentType = contentType;
            var result = new FileContentResult(System.IO.File.ReadAllBytes(optimalSolutionsPath + attachmentFile), contentType)
            {
                FileDownloadName = $"Attachments.zip"
            };

            System.IO.File.Delete(optimalSolutionsPath + attachmentFile);

            return result;
        }

        private string GetOptimalSolutionsPath()
        {
            var optimalSolutionsPath = Path.Combine(Path.GetTempPath(), "RBATOptimalSolutions");
            if (!Directory.Exists(optimalSolutionsPath))
            {
                Directory.CreateDirectory(optimalSolutionsPath);
            }

            optimalSolutionsPath = optimalSolutionsPath + "\\";

            return optimalSolutionsPath;
        }

        public IActionResult SeasonalModel()
        {
            ViewBag.title = "Solver Seasonal Model";
            return View();
        }

        public IActionResult SeasonalReservoirLevel()
        {
            ViewBag.title = "Seasonal Reservoir Levels";
            return View();
        }

        public IActionResult SeasonalDiversionLicense()
        {
            ViewBag.title = "Seasonal Diversion Licenses";
            return View();
        }

        public IActionResult SeasonalApportionmentTarget()
        {
            ViewBag.title = "Seasonal Apportionment Targets";
            return View();
        }

        public IActionResult SeasonalWaterDemand()
        {
            ViewBag.title = "Seasonal Water Demands";
            return View();
        }

        [HttpPost]
        public async Task<SeasonalModel> GetSeasonalData(int projectId, long scenarioId, bool isSeasonalModel, DateTime startDate, int numberOfTimeIntervals, double sensitivityAnalysis, double aridityFactor) {
            return await _solverService.GetSeasonalData(projectId, scenarioId, isSeasonalModel, startDate, numberOfTimeIntervals, sensitivityAnalysis, aridityFactor);
        }       
    }
}