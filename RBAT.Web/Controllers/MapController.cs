using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBAT.Logic.Interfaces;
using RBAT.Core.Models;
using RBAT.Web.Models.Map;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class MapController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly INodeService _nodeService;
        private readonly IChannelService _channelService;

        public MapController(IProjectService projectService, INodeService nodeService, IChannelService channelService)
        {
            this._projectService = projectService;
            this._nodeService = nodeService;
            this._channelService = channelService;
        }

        public IActionResult Index(int? projectId)
        {
            var model = new MapModel
            {
                ProjectId = projectId
            };
            return View(model);
        }

        public JsonResult GetProjectData(int? projectId)
        {
            var model = GetProjectDetails(projectId);
            return Json(model);
        }

        [HttpPost]
        public JsonResult SaveProjectData(MapModel model)
        {
            try
            {
                this._projectService.UpdateProjectMapData(model.ProjectId.Value, model.ProjectMapData);
                foreach (var node in model.ProjectNodes)
                {
                    this._nodeService.UpdateNodeMapData(node.Id, node.MapData);
                }
                foreach (var channel in model.ProjectChannels)
                {
                    this._channelService.UpdateChannelMapData(channel.Id, channel.MapData);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Type = "Error", Message = ex.Message });
            }
            return Json(new { Type = "Success" });
        }

        private MapModel GetProjectDetails(int? projectId)
        {
            var model = new MapModel
            {
                ProjectId = projectId,
                ProjectNodes = new ProjectNodeModelList(),
                ProjectChannels = new ProjectChannelModelList()
            };

            if (projectId.HasValue)
            {
                var project = this._projectService.GetProjectByID(projectId.Value);
                model.ProjectMapData = project.MapData;
                var projectNodes = this._projectService.GetProjectNodes(projectId.Value);
                model.ProjectNodes = projectNodes.ToProjectNodeViewModelList();
                var projectChannels = this._projectService.GetProjectChannels(projectId.Value);
                model.ProjectChannels = projectChannels.ToProjectChannelViewModelList();
            }
            
            return model;
        }
    }
}