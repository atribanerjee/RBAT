using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic.Extensions;
using RBAT.Logic.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{
    public class ChannelZoneLevelRecordedFlowStationService : IChannelZoneLevelRecordedFlowStationService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ChannelZoneLevelRecordedFlowStationService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<ChannelZoneLevelRecordedFlowStation> Get(int channelPolicyGroupId, int channelId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.ChannelZoneLevelRecordedFlowStations
                                .Where(c => c.ChannelPolicyGroupId == channelPolicyGroupId)
                                .Where(c => c.ChannelId == channelId)
                                .FirstOrDefaultAsync();
            }
        }        

        public async Task Save(ChannelZoneLevelRecordedFlowStation channelZoneLevelRecordedFlowStation)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelZoneRFS = await ctx.ChannelZoneLevelRecordedFlowStations.Where(z => z.ChannelPolicyGroupId == channelZoneLevelRecordedFlowStation.ChannelPolicyGroupId)
                                                                                   .Where(z => z.ChannelId == channelZoneLevelRecordedFlowStation.ChannelId)
                                                                                   .FirstOrDefaultAsync();

                if (channelZoneRFS != null)
                {
                    var config = new BulkConfig
                    {
                        PropertiesToInclude = new List<string> {
                            nameof(ChannelZoneLevelRecordedFlowStation.Zone1Id),
                            nameof(ChannelZoneLevelRecordedFlowStation.Zone2Id),
                            nameof(ChannelZoneLevelRecordedFlowStation.Zone3Id),
                            nameof(ChannelZoneLevelRecordedFlowStation.RecordedFlowStation1Id),
                            nameof(ChannelZoneLevelRecordedFlowStation.RecordedFlowStation2Id),
                            nameof(ChannelZoneLevelRecordedFlowStation.RecordedFlowStation3Id)
                        },
                        UpdateByProperties = new List<string> {
                            nameof(ChannelZoneLevelRecordedFlowStation.ChannelPolicyGroupId),
                            nameof(ChannelZoneLevelRecordedFlowStation.ChannelId)
                        }
                    };
                    await ctx.BulkUpdateAsync(new List<ChannelZoneLevelRecordedFlowStation>() { channelZoneLevelRecordedFlowStation }, config);
                }
                else
                {
                    await ctx.BulkInsertAsyncExtended(new List<ChannelZoneLevelRecordedFlowStation>() { channelZoneLevelRecordedFlowStation }, _contextAccessor?.HttpContext?.User?.Identity.Name);
                }
            }
        }
    }    
}
