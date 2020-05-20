using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RBAT.Core;
using RBAT.Core.Clasess;
using RBAT.Core.Models;
using RBAT.Logic.Extensions;
using RBAT.Logic.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{
    public class NodeZoneLevelService : INodeZoneLevelService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public NodeZoneLevelService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<NodeZoneLevel>> GetAll(int nodePolicyGroupID, int nodeID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.NodeZoneLevels
                                .Where(c => c.NodePolicyGroupID == nodePolicyGroupID)
                                .Where(c => c.NodeID == nodeID)
                                .OrderBy(c => c.Year)
                                .ThenBy(c => c.TimeComponentValue)
                                .ToListAsync();
            }
        }        

        public async Task<IList<NodeZoneLevel>> GetJoinedList(int nodePolicyGroupID, int nodeID, IList<NodeZoneLevel> existingList, List<ZoneLevelWithTimeComponent> pasteList)
        {
            var fromPastList = await GetNodeZoneLevelListForPastedData(nodePolicyGroupID, nodeID, pasteList);
            var combinedList = existingList.Union(fromPastList);
            return combinedList.ToList();                        
        }

        public async Task SaveAll(int nodePolicyGroupID, int nodeID, IList<NodeZoneLevel> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var listToDelete = ctx.NodeZoneLevels
                                      .Where(x => x.NodePolicyGroupID == nodePolicyGroupID)
                                      .Where(c => c.NodeID == nodeID)
                                      .ToList();

                await ctx.BulkDeleteAsync(listToDelete);
                await ctx.BulkInsertAsyncExtended(listToSave, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }        

        public async Task UpdateAll(IList<NodeZoneLevel> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsync(listToUpdate);
            }
        }

        public async Task RemoveAll(IList<NodeZoneLevel> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        private async Task<IList<NodeZoneLevel>> GetNodeZoneLevelListForPastedData(int nodePolicyGroupID, int nodeID, List<ZoneLevelWithTimeComponent> pasteList)
        {
            IList<NodeZoneLevel> nodeZoneLevelList = new List<NodeZoneLevel>();
            pasteList.ForEach(item =>
            {
                NodeZoneLevel nodeZoneLevel = SetNodeZoneLevelCoreData(nodePolicyGroupID, nodeID, item);
                SetNodeZoneLevelAboveIdeal(item, nodeZoneLevel);
                SetNodeZoneLevelBelowIdeal(item, nodeZoneLevel);
                nodeZoneLevelList.Add(nodeZoneLevel);
            });

            return await Task.Run(() => nodeZoneLevelList);
        }

        private static NodeZoneLevel SetNodeZoneLevelCoreData(int nodePolicyGroupID, int nodeID, ZoneLevelWithTimeComponent item)
        {
            return new NodeZoneLevel
            {
                NodePolicyGroupID = nodePolicyGroupID,
                NodeID = nodeID,
                Year = item.Year,
                TimeComponentType = (int)TimeComponent.Custom,
                TimeComponentValue = item.TimeComponentValue
            };
        }

        private static void SetNodeZoneLevelAboveIdeal(ZoneLevelWithTimeComponent item, NodeZoneLevel nodeZoneLevel)
        {
            var nodeZoneLevelsAboveIdeal = item.ZoneLevelsAboveIdeal;
            if (nodeZoneLevelsAboveIdeal != null)
            {
                nodeZoneLevelsAboveIdeal.Reverse();
                switch (nodeZoneLevelsAboveIdeal.Count)
                {
                    case 6:
                        nodeZoneLevel.ZoneAboveIdeal6 = nodeZoneLevelsAboveIdeal[5];
                        goto case 5;
                    case 5:
                        nodeZoneLevel.ZoneAboveIdeal5 = nodeZoneLevelsAboveIdeal[4];
                        goto case 4;
                    case 4:
                        nodeZoneLevel.ZoneAboveIdeal4 = nodeZoneLevelsAboveIdeal[3];
                        goto case 3;
                    case 3:
                        nodeZoneLevel.ZoneAboveIdeal3 = nodeZoneLevelsAboveIdeal[2];
                        goto case 2;
                    case 2:
                        nodeZoneLevel.ZoneAboveIdeal2 = nodeZoneLevelsAboveIdeal[1];
                        goto case 1;
                    case 1:
                        nodeZoneLevel.ZoneAboveIdeal1 = nodeZoneLevelsAboveIdeal[0];
                        break;
                }
            }
        }

        private static void SetNodeZoneLevelBelowIdeal(ZoneLevelWithTimeComponent item, NodeZoneLevel nodeZoneLevel)
        {
            var nodeZoneLevelsBelowIdeal = item.ZoneLevelsBelowIdeal;
            if (nodeZoneLevelsBelowIdeal != null)
            {
                switch (nodeZoneLevelsBelowIdeal.Count)
                {
                    case 10:
                        nodeZoneLevel.ZoneBelowIdeal10 = nodeZoneLevelsBelowIdeal[9];
                        goto case 9;
                    case 9:
                        nodeZoneLevel.ZoneBelowIdeal9 = nodeZoneLevelsBelowIdeal[8];
                        goto case 8;
                    case 8:
                        nodeZoneLevel.ZoneBelowIdeal8 = nodeZoneLevelsBelowIdeal[7];
                        goto case 7;
                    case 7:
                        nodeZoneLevel.ZoneBelowIdeal7 = nodeZoneLevelsBelowIdeal[6];
                        goto case 6;
                    case 6:
                        nodeZoneLevel.ZoneBelowIdeal6 = nodeZoneLevelsBelowIdeal[5];
                        goto case 5;
                    case 5:
                        nodeZoneLevel.ZoneBelowIdeal5 = nodeZoneLevelsBelowIdeal[4];
                        goto case 4;
                    case 4:
                        nodeZoneLevel.ZoneBelowIdeal4 = nodeZoneLevelsBelowIdeal[3];
                        goto case 3;
                    case 3:
                        nodeZoneLevel.ZoneBelowIdeal3 = nodeZoneLevelsBelowIdeal[2];
                        goto case 2;
                    case 2:
                        nodeZoneLevel.ZoneBelowIdeal2 = nodeZoneLevelsBelowIdeal[1];
                        goto case 1;
                    case 1:
                        nodeZoneLevel.ZoneBelowIdeal1 = nodeZoneLevelsBelowIdeal[0];
                        break;
                }
            }
        }
    }    
}
