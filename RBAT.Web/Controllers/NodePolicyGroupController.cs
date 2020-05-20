using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using RBAT.Web.Models.NodePolicyGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    public class NodePolicyGroupController : Controller
    {
        private readonly INodePolicyGroupService _nodePolicyGroupService;
        private readonly IScenarioService _scenarioService;        

        public NodePolicyGroupController(INodePolicyGroupService nodePolicyGroupService, 
                                            IScenarioService scenarioService)
        {
            _nodePolicyGroupService = nodePolicyGroupService;
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
            ViewBag.title = "Node Policy Group";
            ViewBag.projectID = scenario?.ProjectID;
            ViewBag.scenarioID = scenario?.Id;
            return View();
        }

        public IActionResult Add(int scenarioID)
        {
            var model = new NodePolicyGroupModel
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
                var nodePolicyGroup = await _nodePolicyGroupService.Get(id);
                var model = nodePolicyGroup.ToNodePolicyGroupModel();
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
                IEnumerable<NodePolicyGroup> nodePolicyGroups = await _nodePolicyGroupService.GetAllByScenarioID(scenarioID);
                var model = nodePolicyGroups.ToNodePolicyGroupViewModelList();
                return Json(new { draw, recordsFiltered = model.Count(), recordsTotal = model.Count(), data = model });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> AddNew(NodePolicyGroup nodePolicyGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _nodePolicyGroupService.Save(nodePolicyGroup);
                    return await Task.FromResult(Json(new { success = true }));
                }
                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<NodePolicyGroup> listToRemove)
        {
            try
            {
                await this._nodePolicyGroupService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new { Success = "True" }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(NodePolicyGroup nodePolicyGroup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listToUpdate = new List<NodePolicyGroup> { nodePolicyGroup };
                    await _nodePolicyGroupService.UpdateAll(listToUpdate);
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
        public IActionResult NodePolicyGroup(int scenarioId)
        {
            ViewBag.title = "Node Policy Group";
            ViewBag.scenarioId = scenarioId;
            return View();
        }

        [HttpGet]
        public IActionResult NodePolicyGroupNode(int nodePolicyGroupId)
        {
            ViewBag.title = "Nodes";
            ViewBag.nodePolicyGroupId = nodePolicyGroupId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetNodePolicyGroupNodes(int nodePolicyGroupId)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                IEnumerable<NodePolicyGroupNode> nodePolicyGroupNode = await _nodePolicyGroupService.GetAllNodes(nodePolicyGroupId);
                var model = nodePolicyGroupNode.ToNodePolicyGroupNodeViewModelList(nodePolicyGroupId);
                int recordsTotal = model.Count();
                return Json(new { draw, recordsFiltered = model.Count(), recordsTotal, data = model });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> NodePolicyGroupNode(int nodePolicyGroupId, int nodeId, bool isChecked)
        {
            if (isChecked)
            {
                var nodeIsAssigned = _nodePolicyGroupService.CheckNodePolicyGroupNode(nodePolicyGroupId, nodeId);
                if (nodeIsAssigned)
                {
                    return Json(new { Type = "NodeError", Message = "The Node is already assigned to other Node Policy Group." });
                }
            }

            var result = await _nodePolicyGroupService.SaveNodePolicyGroupNode(nodePolicyGroupId, nodeId, isChecked);
            return Json(new { Type = result ? "Success" : "Error", Message = "Save Node Policy Group Node: An unexpected error occurred while saving." });
        }
    }
}