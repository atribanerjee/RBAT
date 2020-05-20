using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RBAT.Core;
using RBAT.Core.Clasess;
using RBAT.Core.Interfaces;
using RBAT.Core.Models;
using RBAT.Logic.Extensions;
using RBAT.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{
    public class TimeWaterUseService : ITimeWaterUseService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TimeWaterUseService(IHttpContextAccessor contextAccessor) {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<TimeWaterUse>> GetAllTimeWaterUse(int nodeID, TimeComponent timeComponent)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var waterUse = (from tnf in ctx.TimeWaterUses
                                where tnf.NodeID == nodeID
                                where tnf.TimeComponentType == (int)timeComponent
                                select tnf);

                return await waterUse.ToListAsync();
            }
        }
        
        public async Task<IList<TimeWaterUse>> GetTimeWaterUse(int nodeID, int start, int length, TimeComponent timeComponent = TimeComponent.Month)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var waterUse = (from tnf in ctx.TimeWaterUses                                  
                               where tnf.NodeID == nodeID
                               where tnf.TimeComponentType == (int)timeComponent
                               select tnf)
                               .Skip(start).Take(length);

                return await waterUse.ToListAsync();
            }
        }

        public async Task<int> GetTimeWaterUseTotalCount(int nodeID, TimeComponent timeComponent = TimeComponent.Month)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var recordsCount = ctx.TimeWaterUses
                    .Where(l => l.NodeID == nodeID && l.TimeComponentType == (int)timeComponent)
                    .CountAsync();

                return await recordsCount;
            }
        }

        public async Task<IList<ITimeSeriesItem>> GetJoinedList(int nodeID, DateTime startDate, TimeComponent timeComponent, IList<TimeWaterUse> existingList, List<PastedTimeComponent> pasteList)
        {
            var startDateTime = startDate.GetTimeComponentValueCalculatedFromBeginingOfTheYear(timeComponent);
            var timeSeriesItemService = new TimeSeriesItemService();

            var fromPasteList = await timeSeriesItemService.GenerateTimeSeriesData(nodeID, startDate, timeComponent, pasteList, "TimeWaterUse");
            var fromExistingList = existingList.Where(x => x.Year < startDateTime.year || (x.Year == startDateTime.year && x.TimeComponentValue < startDateTime.timeComponentValue)).ToList();
            var combinedList = fromExistingList.Union(fromPasteList);

            return combinedList.ToList();
        }

        public async Task SaveAll(IList<TimeWaterUse> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task SaveAll(int nodeID, DateTime startDate, TimeComponent timeComponent, IList<TimeWaterUse> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var timeComponentType = (int)timeComponent;
                var listToDelete = ctx.TimeWaterUses.Where(x => x.NodeID == nodeID)
                                                    .Where(x => x.TimeComponentType == timeComponentType)
                                                    .ToList();

                await ctx.BulkDeleteAsync(listToDelete);
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<TimeWaterUse> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task Remove(IList<TimeWaterUse> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        public async Task RemoveAll(int nodeID, TimeComponent timeComponent)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var listToRemove = (from tnf in ctx.TimeWaterUses
                                where tnf.NodeID == nodeID
                                where tnf.TimeComponentType == (int)timeComponent
                                select tnf)
                                .ToList();

                await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        public async Task SaveAllFromFile(int elementID, DateTime startDate, TimeComponent timeComponent, List<PastedTimeComponent> listFromFile, int projectID = 0)
        {
            try
            {
                using (var ctx = new RBATContext(this._contextAccessor))
                {
                    var timeComponentType = (int)timeComponent;
                    var listToDelete = ctx.TimeWaterUses
                                  .Where(x => x.NodeID == elementID)
                                  .Where(x => x.TimeComponentType == timeComponentType)
                                  .ToList();

                    var timeSeriesItemService = new TimeSeriesItemService();
                    var timeSeriesItems = await timeSeriesItemService.GenerateTimeSeriesData(elementID, startDate, timeComponent, listFromFile, "TimeWaterUse");
                    List<TimeWaterUse> listToSave = timeSeriesItems.OfType<TimeWaterUse>().ToList();

                    await ctx.BulkDeleteAsync(listToDelete);
                    await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
