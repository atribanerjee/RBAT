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
    public class RecordedFlowStationService : IRecordedFlowStationService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public RecordedFlowStationService(IHttpContextAccessor contextAccessor) {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<RecordedFlowStation>> GetAll()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.RecordedFlowStations.OrderBy(r => r.Name).ToListAsync();                
            }
        }
        
        public async Task<RecordedFlowStation> GetRecordedFlowStationByID(int id)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.RecordedFlowStations.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task Save(RecordedFlowStation recordedFlowStation)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.RecordedFlowStations.AddAsync(recordedFlowStation);
                ctx.SaveChanges();
            }
        }

        public async Task SaveAll(IList<RecordedFlowStation> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<RecordedFlowStation> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveAll(IList<RecordedFlowStation> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }     
    }
}
