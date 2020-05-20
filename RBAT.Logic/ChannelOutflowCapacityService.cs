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
    public class ChannelOutflowCapacityService : IChannelOutflowCapacityService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public ChannelOutflowCapacityService(IHttpContextAccessor contextAccessor) {
            this._contextAccessor = contextAccessor;
        }
        public async Task<IList<ChannelOutflowCapacity>> GetAll(int channelID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var ChannelOutflowCapacity = from coc in ctx.ChannelOutflowCapacities
                                        where coc.ChannelID == channelID                                       
                                        select coc;

                return await ChannelOutflowCapacity.ToListAsync();
            }
        }

        public async Task<IList<ChannelOutflowCapacity>> GetJoinedList(IList<ChannelOutflowCapacity> existingList, List<ChannelOutflowCapacity> pasteList)
        {                        
            var combinedList = existingList.Union(pasteList).ToAsyncEnumerable();
            return await combinedList.ToList();
        }

        public async Task SaveAll(IList<ChannelOutflowCapacity> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task SaveAll(int channelID, IList<ChannelOutflowCapacity> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var listToDelete = ctx.ChannelOutflowCapacities.Where(x => x.ChannelID == channelID).ToList();

                await ctx.BulkDeleteAsync(listToDelete);
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<ChannelOutflowCapacity> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveAll(IList<ChannelOutflowCapacity> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }
    }
}
