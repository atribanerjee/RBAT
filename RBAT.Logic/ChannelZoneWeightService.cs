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
    public class ChannelZoneWeightService : IChannelZoneWeightService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ChannelZoneWeightService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<ChannelZoneWeight>> GetAll(int channelPolicyGroupID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.ChannelZoneWeights
                                .Where(c => c.ChannelPolicyGroupID == channelPolicyGroupID)
                                .ToListAsync();
            }
        }        

        public async Task<IList<ChannelZoneWeight>> GetJoinedList(int channelPolicyGroupID, IList<ChannelZoneWeight> existingList, List<ZoneLevelWithTimeComponent> pasteList)
        {
            var fromPastList = await GetChannelZoneWeightListForPastedData(channelPolicyGroupID, pasteList);
            var combinedList = existingList.Union(fromPastList);
            return combinedList.ToList();                        
        }

        public async Task Save(ChannelZoneWeight channelZoneWeight)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.AddAsync(channelZoneWeight);
                ctx.SaveChanges();
            }
        }

        public async Task SaveAll(long channelPolicyGroupID, IList<ChannelZoneWeight> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var listToDelete = ctx.ChannelZoneWeights
                                      .Where(x => x.ChannelPolicyGroupID == channelPolicyGroupID)
                                      .ToList();

                await ctx.BulkDeleteAsync(listToDelete);
                await ctx.BulkInsertAsyncExtended(listToSave, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<ChannelZoneWeight> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsync(listToUpdate);
            }
        }

        public async Task RemoveAll(IList<ChannelZoneWeight> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        private async Task<IList<ChannelZoneWeight>> GetChannelZoneWeightListForPastedData(int channelPolicyGroupID, List<ZoneLevelWithTimeComponent> pasteList)
        {
            IList<ChannelZoneWeight> channelZoneWeightList = new List<ChannelZoneWeight>();
            pasteList.ForEach(item =>
            {
                ChannelZoneWeight channelZoneWeight = SetChannelZoneWeightCoreData(channelPolicyGroupID);
                SetChannelZoneWeightAboveIdeal(item, channelZoneWeight);
                SetChannelZoneLevelIdealZone(item, channelZoneWeight);
                SetChannelZoneWeightBelowIdeal(item, channelZoneWeight);
                channelZoneWeightList.Add(channelZoneWeight);
            });

            return await Task.Run(() => channelZoneWeightList);
        }

        private static ChannelZoneWeight SetChannelZoneWeightCoreData(int channelPolicyGroupID)
        {
            return new ChannelZoneWeight
            {
                ChannelPolicyGroupID = channelPolicyGroupID
            };
        }

        private static void SetChannelZoneWeightAboveIdeal(ZoneLevelWithTimeComponent item, ChannelZoneWeight channelZoneWeight)
        {
            var channelZoneLevelsAboveIdeal = item.ZoneLevelsAboveIdeal;
            if (channelZoneLevelsAboveIdeal != null)
            { 
                channelZoneLevelsAboveIdeal.Reverse();
                switch (channelZoneLevelsAboveIdeal.Count)
                {
                    case 6:
                        channelZoneWeight.ZoneAboveIdeal6 = channelZoneLevelsAboveIdeal[5];
                        goto case 5;
                    case 5:
                        channelZoneWeight.ZoneAboveIdeal5 = channelZoneLevelsAboveIdeal[4];
                        goto case 4;
                    case 4:
                        channelZoneWeight.ZoneAboveIdeal4 = channelZoneLevelsAboveIdeal[3];
                        goto case 3;
                    case 3:
                        channelZoneWeight.ZoneAboveIdeal3 = channelZoneLevelsAboveIdeal[2];
                        goto case 2;
                    case 2:
                        channelZoneWeight.ZoneAboveIdeal2 = channelZoneLevelsAboveIdeal[1];
                        goto case 1;
                    case 1:
                        channelZoneWeight.ZoneAboveIdeal1 = channelZoneLevelsAboveIdeal[0];
                        break;
                }
            }
        }

        private static void SetChannelZoneLevelIdealZone(ZoneLevelWithTimeComponent item, ChannelZoneWeight channelZoneWeight)
        {
            channelZoneWeight.IdealZone = 0; /*item.IdealZone[0];*/
        }

        private static void SetChannelZoneWeightBelowIdeal(ZoneLevelWithTimeComponent item, ChannelZoneWeight channelZoneWeight)
        {
            var channelZoneLevelsBelowIdeal = item.ZoneLevelsBelowIdeal;
            if (channelZoneLevelsBelowIdeal != null)
            {
                switch (channelZoneLevelsBelowIdeal.Count)
                {
                    case 10:
                        channelZoneWeight.ZoneBelowIdeal10 = channelZoneLevelsBelowIdeal[9];
                        goto case 9;
                    case 9:
                        channelZoneWeight.ZoneBelowIdeal9 = channelZoneLevelsBelowIdeal[8];
                        goto case 8;
                    case 8:
                        channelZoneWeight.ZoneBelowIdeal8 = channelZoneLevelsBelowIdeal[7];
                        goto case 7;
                    case 7:
                        channelZoneWeight.ZoneBelowIdeal7 = channelZoneLevelsBelowIdeal[6];
                        goto case 6;
                    case 6:
                        channelZoneWeight.ZoneBelowIdeal6 = channelZoneLevelsBelowIdeal[5];
                        goto case 5;
                    case 5:
                        channelZoneWeight.ZoneBelowIdeal5 = channelZoneLevelsBelowIdeal[4];
                        goto case 4;
                    case 4:
                        channelZoneWeight.ZoneBelowIdeal4 = channelZoneLevelsBelowIdeal[3];
                        goto case 3;
                    case 3:
                        channelZoneWeight.ZoneBelowIdeal3 = channelZoneLevelsBelowIdeal[2];
                        goto case 2;
                    case 2:
                        channelZoneWeight.ZoneBelowIdeal2 = channelZoneLevelsBelowIdeal[1];
                        goto case 1;
                    case 1:
                        channelZoneWeight.ZoneBelowIdeal1 = channelZoneLevelsBelowIdeal[0];
                        break;
                }
            }
        }
    }    
}
