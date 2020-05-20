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
    public class TimeRecordedFlowService : ITimeRecordedFlowService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TimeRecordedFlowService(IHttpContextAccessor contextAccessor) {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<TimeRecordedFlow>> GetAllTimeRecordedFlows(int recordedFlowStationID, TimeComponent timeComponent)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var recordedFlow = (from tnf in ctx.TimeRecordedFlows
                                    where tnf.RecordedFlowStationID == recordedFlowStationID
                                    where tnf.TimeComponentType == (int)timeComponent
                                    select tnf);

                return await recordedFlow.ToListAsync();
            }
        }

        public async Task<IList<TimeRecordedFlow>> GetTimeRecordedFlows(int recordedFlowStationID, int start, int length, TimeComponent timeComponent = TimeComponent.Month)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var recordedFlow = (from tnf in ctx.TimeRecordedFlows
                                  where tnf.RecordedFlowStationID == recordedFlowStationID
                                  where tnf.TimeComponentType == (int)timeComponent                                  
                                  select tnf)
                                  .Skip(start).Take(length);

                return await recordedFlow.ToListAsync();
            }
        }

        public async Task<int> GetTimeRecordedFlowsTotalCount(int recordedFlowStationID, TimeComponent timeComponent)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var recordsCount = ctx.TimeRecordedFlows
                    .Where(l => l.RecordedFlowStationID == recordedFlowStationID && l.TimeComponentType == (int)timeComponent)
                    .CountAsync();

                return await recordsCount;
            }
        }

        public async Task<IList<ITimeSeriesItem>> GetJoinedList(int recordedFlowStationID, DateTime startDate, TimeComponent timeComponent, IList<TimeRecordedFlow> existingList, List<PastedTimeComponent> pasteList)
        {
            var startDateTime = startDate.GetTimeComponentValueCalculatedFromBeginingOfTheYear(timeComponent);
            var timeSeriesItemService = new TimeSeriesItemService();

            var fromPasteList = await timeSeriesItemService.GenerateTimeSeriesData(recordedFlowStationID, startDate, timeComponent, pasteList, "TimeRecordedFlow");
            var fromExistingList = existingList.Where(x => x.Year < startDateTime.year || (x.Year == startDateTime.year && x.TimeComponentValue < startDateTime.timeComponentValue)).ToList();
            var combinedList = fromExistingList.Union(fromPasteList);

            return combinedList.ToList();
        }

        public async Task SaveAll(IList<TimeRecordedFlow> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task SaveAll(int recordedFlowStationID, DateTime startDate, TimeComponent timeComponent, IList<TimeRecordedFlow> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var timeComponentType = (int)timeComponent;
                var listToDelete = ctx.TimeRecordedFlows.Where(x => x.RecordedFlowStationID == recordedFlowStationID)
                              .Where(x => x.TimeComponentType == timeComponentType)
                              .ToList();

                await ctx.BulkDeleteAsync(listToDelete);
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<TimeRecordedFlow> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task Remove(IList<TimeRecordedFlow> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);            
            }
        }

        public async Task RemoveAll(int recordedFlowStationID, TimeComponent timeComponent)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var listToRemove = (from tnf in ctx.TimeRecordedFlows
                                    where tnf.RecordedFlowStationID == recordedFlowStationID
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
                    var listToDelete = ctx.TimeRecordedFlows
                                  .Where(x => x.RecordedFlowStationID == elementID)
                                  .Where(x => x.TimeComponentType == timeComponentType)
                                  .ToList();

                    var timeSeriesItemService = new TimeSeriesItemService();
                    var timeSeriesItems = await timeSeriesItemService.GenerateTimeSeriesData(elementID, startDate, timeComponent, listFromFile, "TimeRecordedFlow");
                    List<TimeRecordedFlow> listToSave = timeSeriesItems.OfType<TimeRecordedFlow>().ToList();

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
