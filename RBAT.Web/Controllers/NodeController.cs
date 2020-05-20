using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using RBAT.Web.Models.Node;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class NodeController : Controller
    {

        private readonly INodeService _nodeService;

        public NodeController(INodeService nodeService, ILogger<NodeController> logger)
        {
            this._nodeService = nodeService;
        }

        /// <summary>
        /// Returns the model state errors as JSON.
        /// </summary>
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


        public IActionResult Add(int nodeTypeId)
        {
            var model = new NodeModel();
            model.NodeTypeId = nodeTypeId;            
            return View(model);
        }
        public async Task<IActionResult> AddNew(NodeModel nodeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await this._nodeService.Save(nodeModel.ToNodeEntityModel());
                    return await Task.FromResult(Json(new { success = true }));
                }
                return ModelStateErrors;

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public IActionResult Edit(int id, string name, string description, int nodeTypeID, decimal? sizeOfIrrigatedArea, decimal? landUseFactor, int? measuringUnitId)
        {
            var node = new Node
            {
                Id = id,
                Name = name,
                Description = description,
                NodeTypeId = nodeTypeID,
                SizeOfIrrigatedArea = sizeOfIrrigatedArea,
                LandUseFactor = landUseFactor,
                MeasuringUnitId = measuringUnitId
            };

            return View(node.ToNodeModel());
        }

        public async Task<IActionResult> GetAll(int nodeTypeID)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                IEnumerable<Node> nodes = await this._nodeService.GetNodesByNodeType(nodeTypeID);
                var model = nodes.ToNodeViewModelList();                
                return Json(new { draw, recordsFiltered = model.Count(), recordsTotal = model.Count(), data = model });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public IActionResult Index(int? nodeId)
        {
            ViewBag.title = "Node List";
            ViewBag.selectedNodeId = nodeId;
            ViewBag.selectedNodeTypeId = 0;
            if (nodeId.HasValue)
            {
                var node = this._nodeService.GetNodeByID(nodeId.Value).Result;
                ViewBag.selectedNodeTypeId = node?.NodeTypeId;
            }
            return View();
        }

        public async Task<IActionResult> Remove(List<Node> listToRemove)
        {
            try
            {
                await this._nodeService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new { Success = "True" }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]        
        public async Task<IActionResult> Update(NodeModel nodeModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listToUpdate = new List<Node> { nodeModel.ToNodeEntityModel() };
                    await this._nodeService.UpdateAll(listToUpdate);
                    return Json(new { success = true });
                }

                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}
