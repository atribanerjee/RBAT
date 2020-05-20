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
    public class TimeClimateDataService : ITimeClimateDataService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TimeClimateDataService(IHttpContextAccessor contextAccessor) {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<TimeClimateData>> GetAllTimeClimateData(int climateStationID, TimeComponent timeComponent)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var ClimateData = (from tnf in ctx.TimeClimateDatas
                                   where tnf.ClimateStationID == climateStationID
                                   where tnf.TimeComponentType == (int)timeComponent
                                   select tnf);

                return await ClimateData.ToListAsync();
            }
        }

        public async Task<IList<TimeClimateData>> GetTimeClimateData(int climateStationID, int start, int length, TimeComponent timeComponent = TimeComponent.Month)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var ClimateData = (from tnf in ctx.TimeClimateDatas
                                    where tnf.ClimateStationID == climateStationID
                                    where tnf.TimeComponentType == (int)timeComponent
                                    select tnf)
                                    .Skip(start).Take(length);

                return await ClimateData.ToListAsync();
            }
        }

        public async Task<int> GetTimeClimateDatalTotalCount(int climateStationID, TimeComponent timeComponent)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var recordsCount = ctx.TimeClimateDatas
                    .Where(l => l.ClimateStationID == climateStationID && l.TimeComponentType == (int)timeComponent)
                    .CountAsync();

                return await recordsCount;
            }
        }

        public async Task<IList<ITimeSeriesItem>> GetJoinedList(int climateStationID, DateTime startDate, TimeComponent timeComponent, IList<TimeClimateData> existingList, List<PastedTimeComponent> pasteList)
        {            
            var startDateTime = startDate.GetTimeComponentValueCalculatedFromBeginingOfTheYear(timeComponent);
            var timeSeriesItemService = new TimeSeriesItemService();

            var fromPasteList = await timeSeriesItemService.GenerateTimeSeriesData(climateStationID, startDate, timeComponent, pasteList, "TimeClimateData");
            var fromExistingList = existingList.Where(x => x.Year < startDateTime.year || (x.Year == startDateTime.year && x.TimeComponentValue < startDateTime.timeComponentValue)).ToList();
            var combinedList = fromExistingList.Union(fromPasteList);

            return combinedList.ToList();
        }

        public async Task SaveAll(IList<TimeClimateData> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task SaveAll(int climateStationID, DateTime startDate, TimeComponent timeComponent, IList<TimeClimateData> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var timeComponentType = (int)timeComponent;
                var listToDelete = ctx.TimeClimateDatas.Where(x => x.ClimateStationID == climateStationID)
                                                    .Where(x => x.TimeComponentType == timeComponentType)
                                                    .ToList();

                await ctx.BulkDeleteAsync(listToDelete);
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<TimeClimateData> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task Remove(IList<TimeClimateData> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        public async Task RemoveAll(int climateStationID, TimeComponent timeComponent)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var listToRemove = (from tnf in ctx.TimeClimateDatas
                                    where tnf.ClimateStationID == climateStationID
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
                    var listToDelete = ctx.TimeClimateDatas
                                  .Where(x => x.ClimateStationID == elementID)
                                  .Where(x => x.TimeComponentType == timeComponentType)
                                  .ToList();

                    var timeSeriesItemService = new TimeSeriesItemService();
                    var timeSeriesItems = await timeSeriesItemService.GenerateTimeSeriesData(elementID, startDate, timeComponent, listFromFile, "TimeClimateData");
                    List<TimeClimateData> listToSave = timeSeriesItems.OfType<TimeClimateData>().ToList();

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
