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
    public class ChannelTravelTimeService : IChannelTravelTimeService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public ChannelTravelTimeService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }
        public async Task<IList<ChannelTravelTime>> GetAll(int channelID)
        {
            using (var ctx = new RBATContext( this._contextAccessor ))
            {
                var channelTravelTime = from ctt in ctx.ChannelTravelTimes
                                        where ctt.ChannelID == channelID                                       
                                        select ctt;

                return await channelTravelTime.ToListAsync();
            }
        }

        public async Task<IList<ChannelTravelTime>> GetJoinedList(IList<ChannelTravelTime> existingList, List<ChannelTravelTime> pasteList)
        {                        
            var combinedList = existingList.Union(pasteList).ToAsyncEnumerable();
            return await combinedList.ToList();
        }

        public async Task SaveAll(IList<ChannelTravelTime> listToSave)
        {
            using (var ctx = new RBATContext( ))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task SaveAll(int channelID, IList<ChannelTravelTime> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var listToDelete = ctx.ChannelTravelTimes.Where(x => x.ChannelID == channelID).ToList();

                await ctx.BulkDeleteAsync(listToDelete);
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<ChannelTravelTime> listToUpdate)
        {
            using (var ctx = new RBATContext( ))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveAll(IList<ChannelTravelTime> listToRemove)
        {
            using (var ctx = new RBATContext( ))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }
    }
}
