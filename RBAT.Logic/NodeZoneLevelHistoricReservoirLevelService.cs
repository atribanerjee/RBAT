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
    public class NodeZoneLevelHistoricReservoirLevelService : INodeZoneLevelHistoricReservoirLevelService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public NodeZoneLevelHistoricReservoirLevelService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<NodeZoneLevelHistoricReservoirLevel> Get(int nodePolicyGroupId, int nodeId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.NodeZoneLevelHistoricReservoirLevels
                                .Where(n => n.NodePolicyGroupId == nodePolicyGroupId)
                                .Where(n => n.NodeId == nodeId)
                                .FirstOrDefaultAsync();
            }
        }        

        public async Task Save(NodeZoneLevelHistoricReservoirLevel nodeZoneLevelHistoricReservoirLevel)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var nodeZoneHRL = await ctx.NodeZoneLevelHistoricReservoirLevels.Where(z => z.NodePolicyGroupId == nodeZoneLevelHistoricReservoirLevel.NodePolicyGroupId)
                                                                                .Where(z => z.NodeId == nodeZoneLevelHistoricReservoirLevel.NodeId)
                                                                                .FirstOrDefaultAsync();

                if (nodeZoneHRL != null)
                {
                    var config = new BulkConfig
                    {
                        PropertiesToInclude = new List<string> {
                            nameof(NodeZoneLevelHistoricReservoirLevel.UseHistoricReservoirLevels),
                        },
                        UpdateByProperties = new List<string> {
                            nameof(NodeZoneLevelHistoricReservoirLevel.NodePolicyGroupId),
                            nameof(NodeZoneLevelHistoricReservoirLevel.NodeId)
                        }
                    };
                    await ctx.BulkUpdateAsync(new List<NodeZoneLevelHistoricReservoirLevel>() { nodeZoneLevelHistoricReservoirLevel }, config);
                }
                else
                {
                    await ctx.BulkInsertAsyncExtended(new List<NodeZoneLevelHistoricReservoirLevel>() { nodeZoneLevelHistoricReservoirLevel }, _contextAccessor?.HttpContext?.User?.Identity.Name);
                }
            }
        }
    }    
}
