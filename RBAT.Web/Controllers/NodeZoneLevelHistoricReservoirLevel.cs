using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using System;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    public class NodeZoneLevelHistoricReservoirLevelController : Controller
    {
        private readonly INodeZoneLevelHistoricReservoirLevelService _nodeZoneLevelHistoricReservoirLevelService;

        public NodeZoneLevelHistoricReservoirLevelController(INodeZoneLevelHistoricReservoirLevelService nodeZoneLevelHistoricReservoirLevelService)
        {
            _nodeZoneLevelHistoricReservoirLevelService = nodeZoneLevelHistoricReservoirLevelService;
        }

        public async Task<IActionResult> Save(long nodePolicyGroupId, int nodeId, bool useHistoricReservoirLevels)
        {
            try
            {
                var nodeZoneLevelHistoricReservoirLevel = new NodeZoneLevelHistoricReservoirLevel
                {
                    NodePolicyGroupId = nodePolicyGroupId,
                    NodeId = nodeId,
                    UseHistoricReservoirLevels = useHistoricReservoirLevels
                };

                await _nodeZoneLevelHistoricReservoirLevelService.Save(nodeZoneLevelHistoricReservoirLevel);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
    }
}