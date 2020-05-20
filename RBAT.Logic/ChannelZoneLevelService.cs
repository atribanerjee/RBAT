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
    public class ChannelZoneLevelService : IChannelZoneLevelService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ChannelZoneLevelService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<ChannelZoneLevel>> GetAll(int channelPolicyGroupID, int channelID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.ChannelZoneLevels
                                .Where(c => c.ChannelPolicyGroupID == channelPolicyGroupID)
                                .Where(c => c.ChannelID == channelID)
                                .OrderBy(c => c.Year)
                                .ThenBy(c => c.TimeComponentValue)
                                .ToListAsync();
            }
        }        

        public async Task<IList<ChannelZoneLevel>> GetJoinedList(int channelPolicyGroupID, int channelID, IList<ChannelZoneLevel> existingList, List<ZoneLevelWithTimeComponent> pasteList)
        {
            var fromPastList = await GetChannelZoneLevelListForPastedData(channelPolicyGroupID, channelID, pasteList);
            var combinedList = existingList.Union(fromPastList);
            return combinedList.ToList();                        
        }

        public async Task SaveAll(int channelPolicyGroupID, int channelID, IList<ChannelZoneLevel> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var listToDelete = ctx.ChannelZoneLevels
                                      .Where(x => x.ChannelPolicyGroupID == channelPolicyGroupID)
                                      .Where(c => c.ChannelID == channelID)
                                      .ToList();

                await ctx.BulkDeleteAsync(listToDelete);
                await ctx.BulkInsertAsyncExtended(listToSave, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }        

        public async Task UpdateAll(IList<ChannelZoneLevel> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsync(listToUpdate);
            }
        }

        public async Task RemoveAll(IList<ChannelZoneLevel> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        private async Task<IList<ChannelZoneLevel>> GetChannelZoneLevelListForPastedData(int channelPolicyGroupID, int channelID, List<ZoneLevelWithTimeComponent> pasteList)
        {
            IList<ChannelZoneLevel> channelZoneLevelList = new List<ChannelZoneLevel>();
            pasteList.ForEach(item =>
            {
                ChannelZoneLevel channelZoneLevel = SetChannelZoneLevelCoreData(channelPolicyGroupID, channelID, item);
                SetChannelZoneLevelAboveIdeal(item, channelZoneLevel);
                SetChannelZoneLevelIdealZone(item, channelZoneLevel);
                SetChannelZoneLevelBelowIdeal(item, channelZoneLevel);
                channelZoneLevelList.Add(channelZoneLevel);
            });

            return await Task.Run(() => channelZoneLevelList);
        }

        private static ChannelZoneLevel SetChannelZoneLevelCoreData(int channelPolicyGroupID, int channelID, ZoneLevelWithTimeComponent item)
        {
            return new ChannelZoneLevel
            {
                ChannelPolicyGroupID = channelPolicyGroupID,
                ChannelID = channelID,
                Year = item.Year,
                TimeComponentType = (int)TimeComponent.Day,
                TimeComponentValue = item.TimeComponentValue
            };
        }

        private static void SetChannelZoneLevelAboveIdeal(ZoneLevelWithTimeComponent item, ChannelZoneLevel channelZoneLevel)
        {
            var channelZoneLevelsAboveIdeal = item.ZoneLevelsAboveIdeal;
            if (channelZoneLevelsAboveIdeal != null)
            {
                channelZoneLevelsAboveIdeal.Reverse();
                switch (channelZoneLevelsAboveIdeal.Count)
                {
                    case 6:
                        channelZoneLevel.ZoneAboveIdeal6 = channelZoneLevelsAboveIdeal[5];
                        goto case 5;
                    case 5:
                        channelZoneLevel.ZoneAboveIdeal5 = channelZoneLevelsAboveIdeal[4];
                        goto case 4;
                    case 4:
                        channelZoneLevel.ZoneAboveIdeal4 = channelZoneLevelsAboveIdeal[3];
                        goto case 3;
                    case 3:
                        channelZoneLevel.ZoneAboveIdeal3 = channelZoneLevelsAboveIdeal[2];
                        goto case 2;
                    case 2:
                        channelZoneLevel.ZoneAboveIdeal2 = channelZoneLevelsAboveIdeal[1];
                        goto case 1;
                    case 1:
                        channelZoneLevel.ZoneAboveIdeal1 = channelZoneLevelsAboveIdeal[0];
                        break;
                }
            }
        }

        private static void SetChannelZoneLevelIdealZone(ZoneLevelWithTimeComponent item, ChannelZoneLevel channelZoneLevel)
        {
            channelZoneLevel.IdealZone = item.IdealZone[0];
        }

        private static void SetChannelZoneLevelBelowIdeal(ZoneLevelWithTimeComponent item, ChannelZoneLevel channelZoneLevel)
        {
            var channelZoneLevelsBelowIdeal = item.ZoneLevelsBelowIdeal;
            if (channelZoneLevelsBelowIdeal != null)
            {
                switch (channelZoneLevelsBelowIdeal.Count)
                {
                    case 10:
                        channelZoneLevel.ZoneBelowIdeal10 = channelZoneLevelsBelowIdeal[9];
                        goto case 9;
                    case 9:
                        channelZoneLevel.ZoneBelowIdeal9 = channelZoneLevelsBelowIdeal[8];
                        goto case 8;
                    case 8:
                        channelZoneLevel.ZoneBelowIdeal8 = channelZoneLevelsBelowIdeal[7];
                        goto case 7;
                    case 7:
                        channelZoneLevel.ZoneBelowIdeal7 = channelZoneLevelsBelowIdeal[6];
                        goto case 6;
                    case 6:
                        channelZoneLevel.ZoneBelowIdeal6 = channelZoneLevelsBelowIdeal[5];
                        goto case 5;
                    case 5:
                        channelZoneLevel.ZoneBelowIdeal5 = channelZoneLevelsBelowIdeal[4];
                        goto case 4;
                    case 4:
                        channelZoneLevel.ZoneBelowIdeal4 = channelZoneLevelsBelowIdeal[3];
                        goto case 3;
                    case 3:
                        channelZoneLevel.ZoneBelowIdeal3 = channelZoneLevelsBelowIdeal[2];
                        goto case 2;
                    case 2:
                        channelZoneLevel.ZoneBelowIdeal2 = channelZoneLevelsBelowIdeal[1];
                        goto case 1;
                    case 1:
                        channelZoneLevel.ZoneBelowIdeal1 = channelZoneLevelsBelowIdeal[0];
                        break;
                }
            }
        }
    }    
}
