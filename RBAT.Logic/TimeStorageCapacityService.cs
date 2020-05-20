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
    public class TimeStorageCapacityService : ITimeStorageCapacityService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TimeStorageCapacityService(IHttpContextAccessor contextAccessor) {
            this._contextAccessor = contextAccessor;
        }
        public async Task<IList<TimeStorageCapacity>> GetAll(int nodeID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var storageCapacity = from tsc in ctx.TimeStorageCapacities
                                      where tsc.NodeID == nodeID                                       
                                      select tsc;

                return await storageCapacity.ToListAsync();
            }
        }

        public async Task<IList<TimeStorageCapacity>> GetJoinedList(IList<TimeStorageCapacity> existingList, List<TimeStorageCapacity> pasteList)
        {                        
            var combinedList = existingList.Union(pasteList).ToAsyncEnumerable();
            return await combinedList.ToList();
        }

        public async Task SaveAll(IList<TimeStorageCapacity> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task SaveAll(int nodeID, IList<TimeStorageCapacity> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var listToDelete = ctx.TimeStorageCapacities.Where(x => x.NodeID == nodeID).ToList();

                await ctx.BulkDeleteAsync(listToDelete);
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<TimeStorageCapacity> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveAll(IList<TimeStorageCapacity> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }
    }
}
