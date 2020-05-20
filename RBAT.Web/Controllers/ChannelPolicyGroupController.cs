using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using RBAT.Web.Models.ChannelPolicyGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    public class ChannelPolicyGroupController : Controller
    {
        private readonly IChannelPolicyGroupService _channelPolicyGroupService;
        private readonly IScenarioService _scenarioService;        

        public ChannelPolicyGroupController(IChannelPolicyGroupService channelPolicyGroupService, 
                                            IScenarioService scenarioService)
        {
            _channelPolicyGroupService = channelPolicyGroupService;
            _scenarioService = scenarioService;
        }

        public JsonResult ModelStateErrors
        {
            get
            {
                var errorModel =
                    from x in ModelState.Keys
                    where ModelState[x].Errors.Count > 0
                    select new
                    {
                        key = x,
                        errors = ModelState[x].Errors.Select(y => y.ErrorMessage).ToArray()
                    };
                return Json(new { success = false, errors = errorModel });
            }
        }
        
        public async Task<IActionResult> Index(long? id)
        {            
            var scenario = await _scenarioService.Get(id);
            ViewBag.title = "Channel Policy Group";
            ViewBag.projectID = scenario?.ProjectID;
            ViewBag.scenarioID = scenario?.Id;
            return View();
        }

        public IActionResult Add(int scenarioID)
        {
            var model = new ChannelPolicyGroupModel
            {
                ScenarioID = scenarioID,
                NumberOfZonesAbove = "",
                NumberOfZonesBelow = ""
            };
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var channelPolicyGroup = await _channelPolicyGroupService.Get(id);
                var model = channelPolicyGroup.ToChannelPolicyGroupModel();
                return View(model);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }       

        public async Task<IActionResult> GetAll(int scenarioID)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                IEnumerable<ChannelPolicyGroup> channelPolicyGroups = await _channelPolicyGroupService.GetAllByScenarioID(scenarioID);
                var model = channelPolicyGroups.ToChannelPolicyGroupViewModelList();
                return Json(new { draw, recordsFiltered = model.Count(), recordsTotal = model.Count(), data = model });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> AddNew(ChannelPolicyGroup channelPolicyGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _channelPolicyGroupService.Save(channelPolicyGroup);
                    return await Task.FromResult(Json(new { success = true }));
                }
                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<ChannelPolicyGroup> listToRemove)
        {
            try
            {
                await this._channelPolicyGroupService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new { Success = "True" }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(ChannelPolicyGroup channelPolicyGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listToUpdate = new List<ChannelPolicyGroup> { channelPolicyGroup };
                    await _channelPolicyGroupService.UpdateAll(listToUpdate);
                    return Json(new { success = true });
                }
                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ChannelPolicyGroup(int scenarioId)
        {
            ViewBag.title = "Channel Policy Group";
            ViewBag.scenarioId = scenarioId;
            return View();
        }

        [HttpGet]
        public IActionResult ChannelPolicyGroupChannel(int channelPolicyGroupId)
        {
            ViewBag.title = "Channels";
            ViewBag.channelPolicyGroupId = channelPolicyGroupId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetChannelPolicyGroupChannels(int channelPolicyGroupId)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                IEnumerable<ChannelPolicyGroupChannel> channelPolicyGroupChannel = await _channelPolicyGroupService.GetAllChannels(channelPolicyGroupId);
                var model = channelPolicyGroupChannel.ToChannelPolicyGroupChannelViewModelList(channelPolicyGroupId);
                int recordsTotal = model.Count();
                return Json(new { draw, recordsFiltered = model.Count(), recordsTotal, data = model });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChannelPolicyGroupChannel(int channelPolicyGroupId, int channelId, bool isChecked)
        {
            if (isChecked)
            {
                var nodeIsAssigned = _channelPolicyGroupService.CheckChannelPolicyGroupNode(channelPolicyGroupId, channelId);
                if (nodeIsAssigned)
                {
                    return Json(new { Type = "ChannelError", Message = "The Channel is already assigned to other Channel Policy Group." });
                }
            }

            var result = await _channelPolicyGroupService.SaveChannelPolicyGroupChannel(channelPolicyGroupId, channelId, isChecked);
            return Json(new { Type = result ? "Success" : "Error", Message = "Save Channel Policy Group Channel: An unexpected error occurred while saving." });
        }
    }
}