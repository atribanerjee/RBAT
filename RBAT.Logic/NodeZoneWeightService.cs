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
    public class NodeZoneWeightService : INodeZoneWeightService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public NodeZoneWeightService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<NodeZoneWeight>> GetAll(int nodePolicyGroupID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.NodeZoneWeights
                                .Where(c => c.NodePolicyGroupID == nodePolicyGroupID)
                                .ToListAsync();
            }
        }        

        public async Task<IList<NodeZoneWeight>> GetJoinedList(int nodePolicyGroupID, IList<NodeZoneWeight> existingList, List<ZoneLevelWithTimeComponent> pasteList)
        {
            var fromPastList = await GetNodeZoneWeightListForPastedData(nodePolicyGroupID, pasteList);
            var combinedList = existingList.Union(fromPastList);
            return combinedList.ToList();                        
        }

        public async Task Save(NodeZoneWeight nodeZoneWeight)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.AddAsync(nodeZoneWeight);
                ctx.SaveChanges();
            }
        }

        public async Task SaveAll(long nodePolicyGroupID, IList<NodeZoneWeight> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var listToDelete = ctx.NodeZoneWeights
                                      .Where(x => x.NodePolicyGroupID == nodePolicyGroupID)
                                      .ToList();

                await ctx.BulkDeleteAsync(listToDelete);
                await ctx.BulkInsertAsyncExtended(listToSave, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<NodeZoneWeight> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsync(listToUpdate);
            }
        }

        public async Task RemoveAll(IList<NodeZoneWeight> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        private async Task<IList<NodeZoneWeight>> GetNodeZoneWeightListForPastedData(int nodePolicyGroupID, List<ZoneLevelWithTimeComponent> pasteList)
        {
            IList<NodeZoneWeight> nodeZoneWeightList = new List<NodeZoneWeight>();
            pasteList.ForEach(item =>
            {
                NodeZoneWeight nodeZoneWeight = SetNodeZoneWeightCoreData(nodePolicyGroupID);
                SetNodeZoneWeightAboveIdeal(item, nodeZoneWeight);
                SetNodeZoneWeightBelowIdeal(item, nodeZoneWeight);
                nodeZoneWeightList.Add(nodeZoneWeight);
            });

            return await Task.Run(() => nodeZoneWeightList);
        }

        private static NodeZoneWeight SetNodeZoneWeightCoreData(int nodePolicyGroupID)
        {
            return new NodeZoneWeight
            {
                NodePolicyGroupID = nodePolicyGroupID
            };
        }

        private static void SetNodeZoneWeightAboveIdeal(ZoneLevelWithTimeComponent item, NodeZoneWeight nodeZoneWeight)
        {
            var nodeZoneLevelsAboveIdeal = item.ZoneLevelsAboveIdeal;
            if (nodeZoneLevelsAboveIdeal != null)
            { 
                nodeZoneLevelsAboveIdeal.Reverse();
                switch (nodeZoneLevelsAboveIdeal.Count)
                {
                    case 6:
                        nodeZoneWeight.ZoneAboveIdeal6 = nodeZoneLevelsAboveIdeal[5];
                        goto case 5;
                    case 5:
                        nodeZoneWeight.ZoneAboveIdeal5 = nodeZoneLevelsAboveIdeal[4];
                        goto case 4;
                    case 4:
                        nodeZoneWeight.ZoneAboveIdeal4 = nodeZoneLevelsAboveIdeal[3];
                        goto case 3;
                    case 3:
                        nodeZoneWeight.ZoneAboveIdeal3 = nodeZoneLevelsAboveIdeal[2];
                        goto case 2;
                    case 2:
                        nodeZoneWeight.ZoneAboveIdeal2 = nodeZoneLevelsAboveIdeal[1];
                        goto case 1;
                    case 1:
                        nodeZoneWeight.ZoneAboveIdeal1 = nodeZoneLevelsAboveIdeal[0];
                        break;
                }
            }
        }

        private static void SetNodeZoneWeightBelowIdeal(ZoneLevelWithTimeComponent item, NodeZoneWeight nodeZoneWeight)
        {
            var nodeZoneLevelsBelowIdeal = item.ZoneLevelsBelowIdeal;
            if (nodeZoneLevelsBelowIdeal != null)
            {
                switch (nodeZoneLevelsBelowIdeal.Count)
                {
                    case 10:
                        nodeZoneWeight.ZoneBelowIdeal10 = nodeZoneLevelsBelowIdeal[9];
                        goto case 9;
                    case 9:
                        nodeZoneWeight.ZoneBelowIdeal9 = nodeZoneLevelsBelowIdeal[8];
                        goto case 8;
                    case 8:
                        nodeZoneWeight.ZoneBelowIdeal8 = nodeZoneLevelsBelowIdeal[7];
                        goto case 7;
                    case 7:
                        nodeZoneWeight.ZoneBelowIdeal7 = nodeZoneLevelsBelowIdeal[6];
                        goto case 6;
                    case 6:
                        nodeZoneWeight.ZoneBelowIdeal6 = nodeZoneLevelsBelowIdeal[5];
                        goto case 5;
                    case 5:
                        nodeZoneWeight.ZoneBelowIdeal5 = nodeZoneLevelsBelowIdeal[4];
                        goto case 4;
                    case 4:
                        nodeZoneWeight.ZoneBelowIdeal4 = nodeZoneLevelsBelowIdeal[3];
                        goto case 3;
                    case 3:
                        nodeZoneWeight.ZoneBelowIdeal3 = nodeZoneLevelsBelowIdeal[2];
                        goto case 2;
                    case 2:
                        nodeZoneWeight.ZoneBelowIdeal2 = nodeZoneLevelsBelowIdeal[1];
                        goto case 1;
                    case 1:
                        nodeZoneWeight.ZoneBelowIdeal1 = nodeZoneLevelsBelowIdeal[0];
                        break;
                }
            }
        }
    }    
}
