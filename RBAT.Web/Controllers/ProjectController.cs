using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using RBAT.Web.Models.Project;

namespace RBAT.Web.Controllers {

    [Authorize]
    public class ProjectController : Controller {

        private readonly INodeService _nodeService;
        private readonly IProjectService _projectService;
        private readonly IScenarioService _scenarioService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProjectController(IProjectService projectService, INodeService nodeService, IScenarioService scenarioService, IHttpContextAccessor contextAccessor) {
            this._projectService = projectService;
            this._nodeService = nodeService;
            this._scenarioService = scenarioService;
            this._contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Returns the model state errors as JSON.
        /// </summary>
        public JsonResult ModelStateErrors {
            get {
                var errorModel =
                    from x in ModelState.Keys
                    where ModelState[x].Errors.Count > 0
                    select new {
                        key = x,
                        errors = ModelState[x].Errors.Select( y => y.ErrorMessage ).ToArray()
                    };
                return Json( new { success = false, errors = errorModel } );
            }
        }

        //GET: Project/Edit
        [HttpGet]
        public IActionResult Edit( int id ) {
            var project = this._projectService.GetProjectByID( id );
            var model = project.ToProjectModel();
            return View( model );
        }

        // POST: Node/GetAllProjects
        public IActionResult GetAllProjects( int typeID ) {
            try {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                IEnumerable<Project> projects = this._projectService.GetProjects();
                var model = projects.ToProjectViewModelList();
                int recordsTotal = projects.Count();
                return Json( new { draw, recordsFiltered = model.Count(), recordsTotal, data = model } );
            } catch ( Exception ex ) {
                return Json( new { error = ex.Message } );
            }
        }

        [HttpPost]
        public IActionResult GetProjectNodes( int projectId ) {
            try {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                IEnumerable<ProjectNode> projects = this._projectService.GetAllProjectNode( projectId );
                var model = projects.ToProjectNodeViewModelList( projectId );
                int recordsTotal = projects.Count();
                return Json( new { draw, recordsFiltered = model.Count(), recordsTotal, data = model } );
            } catch ( Exception ex ) {
                return Json( new { error = ex.Message } );
            }
        }

        [HttpPost]
        public IActionResult GetProjectChannels(int projectId)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                IEnumerable<ProjectChannel> projects = this._projectService.GetAllProjectChannel(projectId);
                var model = projects.ToProjectChannelViewModelList(projectId);
                int recordsTotal = projects.Count();
                return Json(new { draw, recordsFiltered = model.Count(), recordsTotal, data = model });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public IActionResult ProjectDetails( ProjectModel viewModel ) {
            try {
                if ( ModelState.IsValid ) {
                    this._projectService.AddProject( viewModel.Name, viewModel.Description, viewModel.CalculationBeginDate, viewModel.CalculationEndDate, viewModel.RoutingOptionTypeId.GetValueOrDefault(), viewModel.RoutingOptionTypeId.GetValueOrDefault() );
                    return Json( new { success = true } );
                }

                return ModelStateErrors;
            } catch ( Exception ex ) {
                return Json( ex.Message );
            }
        }

        // GET: Project/CopyProject
        [HttpGet]
        public async Task<IActionResult> CopyProject(int projectId)
        {
            try
            {
                var isAdmin = _contextAccessor?.HttpContext?.User?.IsInRole("Admin") ?? false;

                if (isAdmin)
                {
                    return Json(new { Type = "Details" });
                }
                else
                {
                    bool result = false;
                    var projectAndNodes = await this._projectService.CopyProject(projectId);
                    if (projectAndNodes != null)
                    {
                        result = await this._scenarioService.CopyScenarios(projectId, projectAndNodes.Item1);
                    }

                    return Json(new { Type = result ? "Success" : "Error" });
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult CopyProjectChooseUser(int projectId)
        {
            try
            {
                var project = _projectService.GetProjectByID(projectId);
                return View(project.ToProjectModel());
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        // POST: Project/CopyProject
        [HttpPost]
        public async Task<IActionResult> CopyProject(int projectId, string username)
        {
            try
            {
                bool result = true;
                var projectAndNodes = await _projectService.CopyProject(projectId, username);
                if (projectAndNodes != null)
                {
                    result = await this._scenarioService.CopyScenarios(projectId, projectAndNodes.Item1, projectAndNodes.Item2, projectAndNodes.Item3, username);
                }

                return Json(new { Type = result ? "Success" : "Error" });
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ProjectDetails() {
            var model = new ProjectModel {
                CalculationBeginDate = DateTime.Now.Date,
                CalculationEndDate = DateTime.Now.Date,
                CalculationBegin = DateTime.Now.ToString( "d", CultureInfo.InvariantCulture ),
                CalculationEnd = DateTime.Now.ToString( "d", CultureInfo.InvariantCulture )
            };
            return View( model );
        }

        [HttpGet]
        public IActionResult ProjectNode( int projectId ) {
            ViewBag.nodeProjectId = projectId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProjectNode( int projectId, int nodeId, bool isChecked ) {
            var result = await this._projectService.SaveProjectNode( projectId,  nodeId,  isChecked);
            return Json(new { Type = result ? "Success" : "Error" });
        }
        [HttpGet]
        public IActionResult ProjectChannel(int projectId)
        {
            ViewBag.channelProjectId = projectId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProjectChannel(int projectId, int channelId, bool isChecked)
        {
            var result = await this._projectService.SaveProjectChannel(projectId, channelId, isChecked);
            return Json(new { Type = result ? "Success" : "Error" });
        }

        public IActionResult Remove( List<Project> listToRemove ) {
            try {
                var result = this._projectService.RemoveProject( listToRemove );
                return Json( new { Type = result ? "Success" : "Error" } );
            } catch ( Exception ex ) {
                return Json( ex.Message );
            }
        }

        [HttpPost]
        public IActionResult Update( ProjectModel viewModel ) {
            try {
                if ( ModelState.IsValid ) {
                    var model = viewModel.ToProjectEntityModel();
                    this._projectService.UpdateProject( model );
                    return Json( new { success = true } );
                }

                return ModelStateErrors;
            } catch ( Exception ex ) {
                return Json( ex.Message );
            }
        }

        public  IActionResult CustomTimeStep(int? projectId)
        {
            ViewBag.title = "Time Steps";
            var project =   this._projectService.GetProjectByID(projectId.GetValueOrDefault(0));
            ViewBag.elementName = (project != null) ? project.Name : string.Empty;
            ViewBag.elementID = projectId;

            return View();
        }
    }
}
