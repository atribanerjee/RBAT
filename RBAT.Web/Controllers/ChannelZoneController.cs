using Microsoft.AspNetCore.Mvc;
using RBAT.Logic.Interfaces;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    public class ChannelZoneController : Controller
    {
        private readonly IChannelPolicyGroupService _channelPolicyGroupService;
        private readonly IChannelZoneLevelRecordedFlowStationService _channelZoneLevelRecordedFlowStationService;
        private readonly IScenarioService _scenarioService;

        public ChannelZoneController(IChannelPolicyGroupService channelPolicyGroupService, IChannelZoneLevelRecordedFlowStationService channelZoneLevelRecordedFlowStationService, IScenarioService scenarioService)
        {
            _channelPolicyGroupService = channelPolicyGroupService;
            _channelZoneLevelRecordedFlowStationService = channelZoneLevelRecordedFlowStationService;
            _scenarioService = scenarioService;
        }        

        public async Task<IActionResult> Index(long? id)
        {
            var scenario = await _scenarioService.Get(id);
            ViewBag.title = "Channel Policy Group";
            ViewBag.projectID = scenario?.ProjectID;
            ViewBag.scenarioID = scenario?.Id;
            return View();
        }

        public IActionResult ChannelPolicyGroup(int scenarioID)
        {
            ViewBag.title = "Channel Policy Group";
            ViewBag.scenarioID = scenarioID;

            return View();
        }

        public IActionResult ChannelPolicyGroupChannel(int channelPolicyGroupID)
        {
            ViewBag.title = "Channels";
            ViewBag.channelPolicyGroupID = channelPolicyGroupID;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ChannelZoneLevel(int channelPolicyGroupID, int channelID)
        {
            var channelPolicyGroup =  await _channelPolicyGroupService.Get(channelPolicyGroupID);

            ViewBag.title = "Channel Zone Levels";
            ViewBag.channelPolicyGroupID = channelPolicyGroupID;
            ViewBag.channelID = channelID;
            ViewBag.numberOfZonesAbove = channelPolicyGroup.NumberOfZonesAboveIdealLevel;
            ViewBag.numberOfZonesBelow = channelPolicyGroup.NumberOfZonesBelowIdealLevel;

            var channelZoneLevelRecorderFlowStation = await _channelZoneLevelRecordedFlowStationService.Get(channelPolicyGroupID, channelID);

            return View(channelZoneLevelRecorderFlowStation);
        }

        public async Task<IActionResult> ChannelZoneWeight(int channelPolicyGroupID)
        {
            var channelPolicyGroup = await _channelPolicyGroupService.Get(channelPolicyGroupID);

            ViewBag.title = "Channel Zone Weight";
            ViewBag.channelPolicyGroupID = channelPolicyGroupID;
            ViewBag.numberOfZonesAbove = channelPolicyGroup.NumberOfZonesAboveIdealLevel;
            ViewBag.numberOfZonesBelow = channelPolicyGroup.NumberOfZonesBelowIdealLevel;

            return View();
        }       
    }
}