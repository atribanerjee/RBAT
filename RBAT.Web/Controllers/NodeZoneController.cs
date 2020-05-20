using Microsoft.AspNetCore.Mvc;
using RBAT.Logic.Interfaces;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    public class NodeZoneController : Controller
    {
        private readonly INodePolicyGroupService _nodePolicyGroupService;
        private readonly INodeZoneLevelHistoricReservoirLevelService _nodeZoneLevelHistoricReservoirLevelService;
        private readonly IScenarioService _scenarioService;

        public NodeZoneController(INodePolicyGroupService nodePolicyGroupService, INodeZoneLevelHistoricReservoirLevelService nodeZoneLevelHistoricReservoirLevelService, IScenarioService scenarioService)
        {
            _nodePolicyGroupService = nodePolicyGroupService;
            _nodeZoneLevelHistoricReservoirLevelService = nodeZoneLevelHistoricReservoirLevelService;
            _scenarioService = scenarioService;
        }        

        public async Task<IActionResult> Index(long? id)
        {
            var scenario = await _scenarioService.Get(id);
            ViewBag.title = "Node Policy Group";
            ViewBag.projectID = scenario?.ProjectID;
            ViewBag.scenarioID = scenario?.Id;
            return View();
        }

        public IActionResult NodePolicyGroup(int scenarioID)
        {
            ViewBag.title = "Node Policy Group";
            ViewBag.scenarioID = scenarioID;

            return View();
        }

        public IActionResult NodePolicyGroupNode(int nodePolicyGroupID)
        {
            ViewBag.title = "Nodes";
            ViewBag.nodePolicyGroupID = nodePolicyGroupID;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> NodeZoneLevel(int nodePolicyGroupID, int nodeID, int nodeTypeID)
        {
            var nodePolicyGroup =  await _nodePolicyGroupService.Get(nodePolicyGroupID);

            ViewBag.title = "Node Zone Levels";
            ViewBag.nodePolicyGroupID = nodePolicyGroupID;
            ViewBag.nodeID = nodeID;
            ViewBag.nodeTypeID = nodeTypeID;
            ViewBag.numberOfZonesAbove = nodePolicyGroup.NumberOfZonesAboveIdealLevel;
            ViewBag.numberOfZonesBelow = nodePolicyGroup.NumberOfZonesBelowIdealLevel;

            var nodeZoneLevelHistoricReservoirLevel = await _nodeZoneLevelHistoricReservoirLevelService.Get(nodePolicyGroupID, nodeID);

            return View(nodeZoneLevelHistoricReservoirLevel);
        }

        public async Task<IActionResult> NodeZoneWeight(int nodePolicyGroupID)
        {
            var nodePolicyGroup = await _nodePolicyGroupService.Get(nodePolicyGroupID);

            ViewBag.title = "Node Zone Weight";
            ViewBag.nodePolicyGroupID = nodePolicyGroupID;
            ViewBag.numberOfZonesAbove = nodePolicyGroup.NumberOfZonesAboveIdealLevel;
            ViewBag.numberOfZonesBelow = nodePolicyGroup.NumberOfZonesBelowIdealLevel;

            return View();
        }       
    }
}