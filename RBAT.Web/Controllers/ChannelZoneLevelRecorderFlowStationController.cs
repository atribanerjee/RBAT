using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using System;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    public class ChannelZoneLevelRecordedFlowStationController : Controller
    {
        private readonly IChannelZoneLevelRecordedFlowStationService _channelZoneLevelRecordedFlowStationService;

        public ChannelZoneLevelRecordedFlowStationController(IChannelZoneLevelRecordedFlowStationService channelZoneLevelRecordedFlowStationService)
        {
            _channelZoneLevelRecordedFlowStationService = channelZoneLevelRecordedFlowStationService;
        }

        public async Task<IActionResult> Save(long channelPolicyGroupId, int channelId, int? zone1Id, int? zone2Id, int? zone3Id, int? recordedFlowStation1Id, int? recordedFlowStation2Id, int? recordedFlowStation3Id)
        {
            try
            {
                zone1Id = recordedFlowStation1Id == null ? null : zone1Id;
                zone2Id = recordedFlowStation2Id == null ? null : zone2Id;
                zone3Id = recordedFlowStation3Id == null ? null : zone3Id;

                var channelZoneLevelRecordedFlow = new ChannelZoneLevelRecordedFlowStation
                {
                    ChannelPolicyGroupId = channelPolicyGroupId,
                    ChannelId = channelId,
                    Zone1Id = zone1Id,
                    Zone2Id = zone2Id,
                    Zone3Id = zone3Id,
                    RecordedFlowStation1Id = recordedFlowStation1Id,
                    RecordedFlowStation2Id = recordedFlowStation2Id,
                    RecordedFlowStation3Id = recordedFlowStation3Id
                };

                await _channelZoneLevelRecordedFlowStationService.Save(channelZoneLevelRecordedFlow);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }
    }
}