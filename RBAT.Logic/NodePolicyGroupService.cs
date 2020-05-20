using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic.Extensions;
using RBAT.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{
    public class NodePolicyGroupService : INodePolicyGroupService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<NodePolicyGroupService> _logger;
        private readonly INodeZoneWeightService _nodeZoneWeightService;

        public NodePolicyGroupService(IHttpContextAccessor contextAccessor, 
                                         ILogger<NodePolicyGroupService> logger,
                                         INodeZoneWeightService nodeZoneWeightService)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _nodeZoneWeightService = nodeZoneWeightService;
        }

        public async Task<IList<NodePolicyGroup>> GetAllByScenarioID(int scenarioId)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var nodePolicyGroup = ctx.NodePolicyGroups
                                            .Include(c => c.NodeType)
                                            .Where(c => c.ScenarioID == scenarioId)
                                            .OrderBy(c => c.Id);

                return await nodePolicyGroup.ToListAsync();
            }
        }

        public async Task<NodePolicyGroup> Get(int nodePolicyGroupID)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                return await ctx.NodePolicyGroups
                                .Include(c => c.NodeType)
                                .Where(c => c.Id == nodePolicyGroupID)
                                .FirstOrDefaultAsync();
            }
        }

        public async Task Save(NodePolicyGroup nodePolicyGroup)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                await ctx.NodePolicyGroups.AddAsync(nodePolicyGroup);
                ctx.SaveChanges();               
            }
        }

        public async Task SaveAll(IList<NodePolicyGroup> listToSave)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<NodePolicyGroup> listToUpdate)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                await CheckIfNumberOfZonesIsDecreased(listToUpdate);
                await ctx.BulkUpdateAsyncExtended(listToUpdate, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveAll(IList<NodePolicyGroup> listToRemove)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
               foreach (var nodePolicyGroup in listToRemove)
               {
                  var nodeZoneLevels = ctx.NodeZoneLevels.Where(n => n.NodePolicyGroupID == nodePolicyGroup.Id).ToList();
                  await ctx.BulkDeleteAsync(nodeZoneLevels);
               }

               await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        public async Task<IList<NodePolicyGroupNode>> GetAllNodes(int nodePolicyGroupID)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var nodePolicyGroupNodeList = new List<NodePolicyGroupNode>();

                var nodes = from npg in ctx.NodePolicyGroups
                            join sc in ctx.Scenarios on npg.ScenarioID equals sc.Id
                            join pn in ctx.ProjectNodes on sc.ProjectID equals pn.ProjectId
                            join n in ctx.Nodes on pn.NodeId equals n.Id
                            where npg.Id == nodePolicyGroupID
                            where n.NodeTypeId == npg.NodeTypeID
                            select n;

                foreach (var node in nodes)
                {
                    var nodePolicyGroupNodeExists = ctx.NodePolicyGroupNodes
                                                             .Where(c => c.NodePolicyGroupID == nodePolicyGroupID)
                                                             .Where(c => c.NodeID == node.Id)
                                                             .Any();

                    var nodePolicyGroupNode = ctx.NodePolicyGroupNodes
                                                             .Where(c => c.NodePolicyGroupID == nodePolicyGroupID)
                                                             .Where(c => c.NodeID == node.Id)
                                                             .FirstOrDefault();

                    nodePolicyGroupNodeList.Add(new NodePolicyGroupNode
                    {
                        Node = node,
                        NodeID = node.Id,
                        NodePolicyGroupID = nodePolicyGroupNodeExists ? nodePolicyGroupID : 0,
                        Priority = (nodePolicyGroupNode != null) ? nodePolicyGroupNode.Priority : 1000
                    });
                }

                return await Task.FromResult(nodePolicyGroupNodeList.OrderBy(n => n.Priority).ToList());
            }
        }


        public async Task<bool> SaveNodePolicyGroupNode(int nodePolicyGroupID, int nodeID, bool isChecked)
        {
            try
            {
                using (var ctx = new RBATContext(_contextAccessor))
                {
                    var nodePolicyGroupNode = ctx.NodePolicyGroupNodes
                                                       .FirstOrDefault(c => c.NodeID == nodeID && c.NodePolicyGroupID == nodePolicyGroupID);

                    if (!isChecked && nodePolicyGroupNode != null)
                    {
                        var nodeZoneLevel = ctx.NodeZoneLevels
                                               .FirstOrDefault(n => n.NodeID == nodeID && n.NodePolicyGroupID == nodePolicyGroupID);

                        if (nodeZoneLevel != null) ctx.NodeZoneLevels.Remove(nodeZoneLevel);
                        ctx.NodePolicyGroupNodes.Remove(nodePolicyGroupNode);

                        var nodeZoneLevelHistoricReservoirLevel = ctx.NodeZoneLevelHistoricReservoirLevels
                                                                     .FirstOrDefault(n => n.NodeId == nodeID && n.NodePolicyGroupId == nodePolicyGroupID);
                        if (nodeZoneLevelHistoricReservoirLevel != null)
                        {
                            ctx.NodeZoneLevelHistoricReservoirLevels.Remove(nodeZoneLevelHistoricReservoirLevel);
                        }

                        var otherNodePolicyGroupNodes = ctx.NodePolicyGroupNodes
                                                           .Where(c => c.NodeID != nodeID && c.NodePolicyGroupID == nodePolicyGroupID)
                                                           .OrderBy(c => c.Priority)
                                                           .ToList();

                        var priority = 1;

                        otherNodePolicyGroupNodes.ForEach(o => {
                           o.Priority = priority;
                           ctx.NodePolicyGroupNodes.Update(o);
                           priority += 1;
                        });

                        ctx.SaveChanges();

                        return await Task.FromResult(true);
                    }

                    if (isChecked && nodePolicyGroupNode == null)
                    {
                        var priority = ctx.NodePolicyGroupNodes
                                          .Where(c => c.NodePolicyGroupID == nodePolicyGroupID)
                                          .Select(c => c.Priority)
                                          .DefaultIfEmpty(0)
                                          .Max();

                        ctx.NodePolicyGroupNodes.Add(new NodePolicyGroupNode
                        {
                            NodeID = nodeID,
                            NodePolicyGroupID = nodePolicyGroupID,
                            Priority = priority + 1
                        });

                        ctx.SaveChanges();                       

                        return await Task.FromResult(true);
                    }
                }

                return await Task.FromResult(false);
            }
            catch (Exception e)
            {
                this._logger.LogError(string.Format("Save Node Policy Group Node: An unexpected error occurred while saving {0}", e.Message));
                return await Task.FromResult(false);
            }
        }
        //Check if the node has been already assigned in node policy group
        public bool CheckNodePolicyGroupNode(int nodePolicyGroupID, int nodeId)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var nodePolicyGroup = ctx.NodePolicyGroups
                    .Include(x => x.Scenario)
                    .FirstOrDefault(c => c.Id == nodePolicyGroupID);

                if (nodePolicyGroup?.Scenario != null)
                {
                    var nodePolicyGroups = ctx.NodePolicyGroups
                        .Include(x=>x.NodePolicyGroupNodes)
                        .Where(x => x.ScenarioID == nodePolicyGroup.Scenario.Id);

                    foreach (var policyGroup in nodePolicyGroups)
                    {
                        foreach (var nodePolicyGroupNodes in policyGroup.NodePolicyGroupNodes)
                        {
                            if (nodePolicyGroupNodes.NodeID == nodeId)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private async Task CheckIfNumberOfZonesIsDecreased(IList<NodePolicyGroup> listToUpdate)
        {
            var updatedNodePolicyGroup = listToUpdate[0];

            using (var ctx = new RBATContext(_contextAccessor))
            {
                var existingNodePolicyGroup = ctx.NodePolicyGroups.FirstOrDefault(c => c.Id == updatedNodePolicyGroup.Id);

                if (updatedNodePolicyGroup.NumberOfZonesAboveIdealLevel < existingNodePolicyGroup.NumberOfZonesAboveIdealLevel ||
                    updatedNodePolicyGroup.NumberOfZonesBelowIdealLevel < existingNodePolicyGroup.NumberOfZonesBelowIdealLevel)
                    await RemoveExcessiveZonesLevelAndWeights(updatedNodePolicyGroup);
            }
        }

        private async Task RemoveExcessiveZonesLevelAndWeights(NodePolicyGroup updatedNodePolicyGroup)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var zoneLevels = ctx.NodeZoneLevels.Where(c => c.NodePolicyGroupID == updatedNodePolicyGroup.Id).ToList();
                zoneLevels.ForEach(z => SetNullValueToExcessiveZoneLevels(z, updatedNodePolicyGroup));
                await ctx.BulkUpdateAsyncExtended(zoneLevels, _contextAccessor?.HttpContext?.User?.Identity.Name);

                var zoneWeights = ctx.NodeZoneWeights.Where(c => c.NodePolicyGroupID == updatedNodePolicyGroup.Id).ToList();
                zoneWeights.ForEach(z => SetNullValueToExcessiveZoneWeights(z, updatedNodePolicyGroup));
                await ctx.BulkUpdateAsyncExtended(zoneWeights, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        private void SetNullValueToExcessiveZoneLevels(NodeZoneLevel zoneLevel, NodePolicyGroup updatedNodePolicyGroup)
        {
            SetNullValueToExcessiveZoneLevelsAbove(zoneLevel, updatedNodePolicyGroup.NumberOfZonesAboveIdealLevel);
            SetNullValueToExcessiveZoneLevelsBelow(zoneLevel, updatedNodePolicyGroup.NumberOfZonesBelowIdealLevel);
        }

        private void SetNullValueToExcessiveZoneLevelsAbove(NodeZoneLevel zoneLevel, int numberOfZonesAboveIdealLevel)
        {
            switch (numberOfZonesAboveIdealLevel)
            {
                case 0:
                    zoneLevel.ZoneAboveIdeal1 = null;
                    goto case 1;
                case 1:
                    zoneLevel.ZoneAboveIdeal2 = null;
                    goto case 2;
                case 2:
                    zoneLevel.ZoneAboveIdeal3 = null;
                    goto case 3;
                case 3:
                    zoneLevel.ZoneAboveIdeal4 = null;
                    goto case 4;
                case 4:
                    zoneLevel.ZoneAboveIdeal5 = null;
                    goto case 5;
                case 5:
                    zoneLevel.ZoneAboveIdeal6 = null;
                    break;
            }
        }

        private void SetNullValueToExcessiveZoneLevelsBelow(NodeZoneLevel zoneLevel, int numberOfZonesBelowIdealLevel)
        {
            switch (numberOfZonesBelowIdealLevel)
            {
                case 0:
                    zoneLevel.ZoneBelowIdeal1 = null;
                    goto case 1;
                case 1:
                    zoneLevel.ZoneBelowIdeal2 = null;
                    goto case 2;
                case 2:
                    zoneLevel.ZoneBelowIdeal3 = null;
                    goto case 3;
                case 3:
                    zoneLevel.ZoneBelowIdeal4 = null;
                    goto case 4;
                case 4:
                    zoneLevel.ZoneBelowIdeal5 = null;
                    goto case 5;
                case 5:
                    zoneLevel.ZoneBelowIdeal6 = null;
                    goto case 6;
                case 6:
                    zoneLevel.ZoneBelowIdeal7 = null;
                    goto case 7;
                case 7:
                    zoneLevel.ZoneBelowIdeal8 = null;
                    goto case 8;
                case 8:
                    zoneLevel.ZoneBelowIdeal9 = null;
                    goto case 9;
                case 9:
                    zoneLevel.ZoneBelowIdeal10 = null;
                    break;
            }
        }

        private void SetNullValueToExcessiveZoneWeights(NodeZoneWeight zoneWeight, NodePolicyGroup updatedNodePolicyGroup)
        {
            SetNullValueToExcessiveZoneWeightsAbove(zoneWeight, updatedNodePolicyGroup.NumberOfZonesAboveIdealLevel);
            SetNullValueToExcessiveZoneWeightsBelow(zoneWeight, updatedNodePolicyGroup.NumberOfZonesBelowIdealLevel);
        }

        private void SetNullValueToExcessiveZoneWeightsAbove(NodeZoneWeight zoneWeight, int numberOfZonesAboveIdealLevel)
        {
            switch (numberOfZonesAboveIdealLevel)
            {
                case 0:
                    zoneWeight.ZoneAboveIdeal1 = null;
                    goto case 1;
                case 1:
                    zoneWeight.ZoneAboveIdeal2 = null;
                    goto case 2;
                case 2:
                    zoneWeight.ZoneAboveIdeal3 = null;
                    goto case 3;
                case 3:
                    zoneWeight.ZoneAboveIdeal4 = null;
                    goto case 4;
                case 4:
                    zoneWeight.ZoneAboveIdeal5 = null;
                    goto case 5;
                case 5:
                    zoneWeight.ZoneAboveIdeal6 = null;
                    break;
            }
        }

        private void SetNullValueToExcessiveZoneWeightsBelow(NodeZoneWeight zoneWeight, int numberOfZonesBelowIdealLevel)
        {
            switch (numberOfZonesBelowIdealLevel)
            {
                case 0:
                    zoneWeight.ZoneBelowIdeal1 = null;
                    goto case 1;
                case 1:
                    zoneWeight.ZoneBelowIdeal2 = null;
                    goto case 2;
                case 2:
                    zoneWeight.ZoneBelowIdeal3 = null;
                    goto case 3;
                case 3:
                    zoneWeight.ZoneBelowIdeal4 = null;
                    goto case 4;
                case 4:
                    zoneWeight.ZoneBelowIdeal5 = null;
                    goto case 5;
                case 5:
                    zoneWeight.ZoneBelowIdeal6 = null;
                    goto case 6;
                case 6:
                    zoneWeight.ZoneBelowIdeal7 = null;
                    goto case 7;
                case 7:
                    zoneWeight.ZoneBelowIdeal8 = null;
                    goto case 8;
                case 8:
                    zoneWeight.ZoneBelowIdeal9 = null;
                    goto case 9;
                case 9:
                    zoneWeight.ZoneBelowIdeal10 = null;
                    break;
            }
        }
    }
}
