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
    public class ChannelPolicyGroupService : IChannelPolicyGroupService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<ChannelPolicyGroupService> _logger;
        private readonly IChannelZoneWeightService _channelZoneWeightService;

        public ChannelPolicyGroupService(IHttpContextAccessor contextAccessor,
                                         ILogger<ChannelPolicyGroupService> logger,
                                         IChannelZoneWeightService channelZoneWeightService)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _channelZoneWeightService = channelZoneWeightService;
        }

        public async Task<IList<ChannelPolicyGroup>> GetAllByScenarioID(int scenarioId)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var channelPolicyGroup = ctx.ChannelPolicyGroups
                                            .Include(c => c.ChannelType)
                                            .Where(c => c.ScenarioID == scenarioId)
                                            .OrderBy(c => c.Id);

                return await channelPolicyGroup.ToListAsync();
            }
        }

        public async Task<ChannelPolicyGroup> Get(int channelPolicyGroupID)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                return await ctx.ChannelPolicyGroups
                                .Include(c => c.ChannelType)
                                .Where(c => c.Id == channelPolicyGroupID)
                                .FirstOrDefaultAsync();
            }
        }

        public async Task Save(ChannelPolicyGroup channelPolicyGroup)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                await ctx.ChannelPolicyGroups.AddAsync(channelPolicyGroup);
                ctx.SaveChanges();

                if (channelPolicyGroup.ChannelTypeID == 2) /*diversion channel type*/
                {
                    await _channelZoneWeightService.Save(new ChannelZoneWeight {
                        ChannelPolicyGroupID = channelPolicyGroup.Id,
                        IdealZone = 0
                    });
                }
            }
        }

        public async Task SaveAll(IList<ChannelPolicyGroup> listToSave)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<ChannelPolicyGroup> listToUpdate)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                await CheckIfNumberOfZonesIsDecreased(listToUpdate);
                await ctx.BulkUpdateAsyncExtended(listToUpdate, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveAll(IList<ChannelPolicyGroup> listToRemove)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        public async Task<IList<ChannelPolicyGroupChannel>> GetAllChannels(int channelPolicyGroupID)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var channelPolicyGroupChannelList = new List<ChannelPolicyGroupChannel>();

                var channels = from cpg in ctx.ChannelPolicyGroups
                               join sc in ctx.Scenarios on cpg.ScenarioID equals sc.Id
                               join pc in ctx.ProjectChannels on sc.ProjectID equals pc.ProjectId
                               join c in ctx.Channels on pc.ChannelId equals c.Id
                               where cpg.Id == channelPolicyGroupID
                               where c.ChannelTypeId == cpg.ChannelTypeID
                               select c;

                foreach (var channel in channels)
                {
                    var channelPolicyGroupChannelExists = ctx.ChannelPolicyGroupChannels
                                                             .Where(c => c.ChannelPolicyGroupID == channelPolicyGroupID)
                                                             .Where(c => c.ChannelID == channel.Id)
                                                             .Any();

                    channelPolicyGroupChannelList.Add(new ChannelPolicyGroupChannel
                    {
                        Channel = channel,
                        ChannelID = channel.Id,
                        ChannelPolicyGroupID = channelPolicyGroupChannelExists ? channelPolicyGroupID : 0
                    });
                }

                return await Task.FromResult(channelPolicyGroupChannelList);
            }
        }

        public async Task<bool> SaveChannelPolicyGroupChannel(int channelPolicyGroupID, int channelID, bool isChecked)
        {
            try
            {
                using (var ctx = new RBATContext(_contextAccessor))
                {
                    var channelPolicyGroupChannel = ctx.ChannelPolicyGroupChannels
                                                       .FirstOrDefault(c => c.ChannelID == channelID && c.ChannelPolicyGroupID == channelPolicyGroupID);

                    if (!isChecked && channelPolicyGroupChannel != null)
                    {
                        var channelZoneLevel = ctx.ChannelZoneLevels
                                                  .FirstOrDefault(c => c.ChannelID == channelID && c.ChannelPolicyGroupID == channelPolicyGroupID);

                        if (channelZoneLevel != null) ctx.ChannelZoneLevels.Remove(channelZoneLevel);
                        ctx.ChannelPolicyGroupChannels.Remove(channelPolicyGroupChannel);

                        var channelZoneLevelRecordedFlowStations = ctx.ChannelZoneLevelRecordedFlowStations
                                                                      .FirstOrDefault(c => c.ChannelId == channelID && c.ChannelPolicyGroupId == channelPolicyGroupID);

                        ctx.ChannelZoneLevelRecordedFlowStations.Remove(channelZoneLevelRecordedFlowStations);

                        var otherChannelPolicyGroupChannels = ctx.ChannelPolicyGroupChannels
                                                                 .Where(c => c.ChannelID != channelID && c.ChannelPolicyGroupID == channelPolicyGroupID)
                                                                 .OrderBy(c => c.Priority)
                                                                 .ToList();

                        var priority = 1;

                        otherChannelPolicyGroupChannels.ForEach(o => {
                           o.Priority = priority;
                           ctx.ChannelPolicyGroupChannels.Update(o);
                           priority += 1;
                        });

                        ctx.SaveChanges();

                        return await Task.FromResult(true);
                    }

                    if (isChecked && channelPolicyGroupChannel == null)
                    {
                        var priority = ctx.ChannelPolicyGroupChannels
                                          .Where(c => c.ChannelPolicyGroupID == channelPolicyGroupID)
                                          .Select(c => c.Priority)
                                          .DefaultIfEmpty(0)
                                          .Max();

                        ctx.ChannelPolicyGroupChannels.Add(new ChannelPolicyGroupChannel
                        {
                            ChannelID = channelID,
                            ChannelPolicyGroupID = channelPolicyGroupID,
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
                this._logger.LogError(string.Format("Save Channel Policy Group Channel: An unexpected error occurred while saving {0}", e.Message));
                return await Task.FromResult(false);
            }
        }

        public bool CheckChannelPolicyGroupNode(int channelPolicyGroupID, int channelId)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var channelPolicyGroup = ctx.ChannelPolicyGroups
                    .Include(x => x.Scenario)
                    .FirstOrDefault(c => c.Id == channelPolicyGroupID);

                if (channelPolicyGroup?.Scenario != null)
                {
                    var channelPolicyGroups = ctx.ChannelPolicyGroups
                        .Include(x => x.ChannelPolicyGroupChannels)
                        .Where(x => x.ScenarioID == channelPolicyGroup.Scenario.Id);

                    foreach (var policyGroup in channelPolicyGroups)
                    {
                        foreach (var channelPolicyGroupChannels in policyGroup.ChannelPolicyGroupChannels)
                        {
                            if (channelPolicyGroupChannels.ChannelID == channelId)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private async Task CheckIfNumberOfZonesIsDecreased(IList<ChannelPolicyGroup> listToUpdate)
        {
            var updatedChannelPolicyGroup = listToUpdate[0];

            using (var ctx = new RBATContext(_contextAccessor))
            {
                var existingChannelPolicyGroup = ctx.ChannelPolicyGroups.FirstOrDefault(c => c.Id == updatedChannelPolicyGroup.Id);

                if (updatedChannelPolicyGroup.NumberOfZonesAboveIdealLevel < existingChannelPolicyGroup.NumberOfZonesAboveIdealLevel ||
                    updatedChannelPolicyGroup.NumberOfZonesBelowIdealLevel < existingChannelPolicyGroup.NumberOfZonesBelowIdealLevel)
                    await RemoveExcessiveZonesLevelAndWeights(updatedChannelPolicyGroup);
            }
        }

        private async Task RemoveExcessiveZonesLevelAndWeights(ChannelPolicyGroup updatedChannelPolicyGroup)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var zoneLevels = ctx.ChannelZoneLevels.Where(c => c.ChannelPolicyGroupID == updatedChannelPolicyGroup.Id).ToList();
                zoneLevels.ForEach(z => SetNullValueToExcessiveZoneLevels(z, updatedChannelPolicyGroup));
                await ctx.BulkUpdateAsyncExtended(zoneLevels, _contextAccessor?.HttpContext?.User?.Identity.Name);

                var zoneWeights = ctx.ChannelZoneWeights.Where(c => c.ChannelPolicyGroupID == updatedChannelPolicyGroup.Id).ToList();
                zoneWeights.ForEach(z => SetNullValueToExcessiveZoneWeights(z, updatedChannelPolicyGroup));
                await ctx.BulkUpdateAsyncExtended(zoneWeights, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        private void SetNullValueToExcessiveZoneLevels(ChannelZoneLevel zoneLevel, ChannelPolicyGroup updatedChannelPolicyGroup)
        {
            SetNullValueToExcessiveZoneLevelsAbove(zoneLevel, updatedChannelPolicyGroup.NumberOfZonesAboveIdealLevel);
            SetNullValueToExcessiveZoneLevelsBelow(zoneLevel, updatedChannelPolicyGroup.NumberOfZonesBelowIdealLevel);
        }

        private void SetNullValueToExcessiveZoneLevelsAbove(ChannelZoneLevel zoneLevel, int numberOfZonesAboveIdealLevel)
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

        private void SetNullValueToExcessiveZoneLevelsBelow(ChannelZoneLevel zoneLevel, int numberOfZonesBelowIdealLevel)
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

        private void SetNullValueToExcessiveZoneWeights(ChannelZoneWeight zoneWeight, ChannelPolicyGroup updatedChannelPolicyGroup)
        {
            SetNullValueToExcessiveZoneWeightsAbove(zoneWeight, updatedChannelPolicyGroup.NumberOfZonesAboveIdealLevel);
            SetNullValueToExcessiveZoneWeightsBelow(zoneWeight, updatedChannelPolicyGroup.NumberOfZonesBelowIdealLevel);
        }

        private void SetNullValueToExcessiveZoneWeightsAbove(ChannelZoneWeight zoneWeight, int numberOfZonesAboveIdealLevel)
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

        private void SetNullValueToExcessiveZoneWeightsBelow(ChannelZoneWeight zoneWeight, int numberOfZonesBelowIdealLevel)
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