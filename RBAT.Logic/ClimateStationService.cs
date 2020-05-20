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

    public class ClimateStationService : IClimateStationService {

        private readonly IHttpContextAccessor _contextAccessor;
        public ClimateStationService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<ClimateStation>> GetAll()
        {
            using (var ctx = new RBATContext( this._contextAccessor ))
            {
                return await ctx.ClimateStations.OrderBy(c => c.Name).ToListAsync();
            }
        }

        public async Task<ClimateStation> GetClimateStationByID(int climateStationId)
        {
            using (var ctx = new RBATContext( this._contextAccessor ))
            {
                return await ctx.ClimateStations.FirstOrDefaultAsync(x => x.Id == climateStationId);
            }
        }

        public async Task Save(ClimateStation cimateStation) {
            using ( var ctx = new RBATContext( this._contextAccessor ) ) {
                await ctx.ClimateStations.AddAsync(cimateStation);
                ctx.SaveChanges();
            }
        }

        public async Task UpdateAll(IList<ClimateStation> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveAll(IList<ClimateStation> listToRemove)
        {
            using (var ctx = new RBATContext( this._contextAccessor ))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }
    }
}
