using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic.Extensions;
using RBAT.Logic.Interfaces;
using RBAT.Logic.TransferModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{
    public class NetEvaporationService : INetEvaporationService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public NetEvaporationService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<NetEvaporationModel>> GetAll(int nodeID)
        {
            using (var ctx = new RBATContext( this._contextAccessor ))
            {
                var netEvaporation = from ne in ctx.NetEvaporations
                                     join cs in ctx.ClimateStations
                                     on ne.ClimateStationID equals cs.Id
                                     where ne.NodeID == nodeID                                    
                                     select new NetEvaporationModel
                                     {
                                         Id = ne.Id,
                                         ClimateStationId = ne.ClimateStationID,
                                         ClimateStationName = cs.Name,
                                         AdjustmentFactor = ne.AdjustmentFactor,
                                         NodeId = ne.NodeID
                                     };

                return await netEvaporation.ToListAsync();
            }
        }       

        public async Task SaveAll(IList<NetEvaporation> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }   

        public async Task Update(NetEvaporation item)
        {
            using (var ctx = new RBATContext( this._contextAccessor ))
            {
                var netEvaporation = ctx.NetEvaporations.Where(n => n.Id == item.Id).FirstOrDefault();
                netEvaporation.ClimateStationID = item.ClimateStationID;
                netEvaporation.AdjustmentFactor = item.AdjustmentFactor;
                var listToUpdate = new List<NetEvaporation> { netEvaporation };
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveAll(IList<NetEvaporation> listToRemove)
        {
            using (var ctx = new RBATContext( this._contextAccessor ))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }
    }
}
