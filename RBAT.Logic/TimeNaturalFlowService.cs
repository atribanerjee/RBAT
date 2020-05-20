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
    public class TimeNaturalFlowService : ITimeNaturalFlowService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TimeNaturalFlowService(IHttpContextAccessor contextAccessor) {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<TimeNaturalFlow>> GetAllTimeNaturalFlows(int nodeID, TimeComponent timeComponent, int projectID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var naturalFlow = (from tnf in ctx.TimeNaturalFlows
                                   where tnf.ProjectID == projectID
                                   where tnf.NodeID == nodeID
                                   where tnf.TimeComponentType == (int)timeComponent
                                   select tnf);

                return await naturalFlow.ToListAsync();
            }
        }

        public async Task<IList<TimeNaturalFlow>> GetTimeNaturalFlows(int projectID, int nodeID, int start, int length, TimeComponent timeComponent = TimeComponent.Month)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var naturalFlow = (from tnf in ctx.TimeNaturalFlows
                                  where tnf.ProjectID == projectID
                                  where tnf.NodeID == nodeID
                                  where tnf.TimeComponentType == (int)timeComponent                                  
                                  select tnf)
                                  .Skip(start).Take(length);

                return await naturalFlow.ToListAsync();
            }
        }            
        
        public async Task<int> GetTimeNaturalFlowsTotalCount(int projectID, int nodeID, TimeComponent timeComponent)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var recordsCount = ctx.TimeNaturalFlows
                    .Where(l => l.ProjectID == projectID && l.NodeID == nodeID && l.TimeComponentType == (int)timeComponent)
                    .CountAsync();

                return await recordsCount;
            }
        }

        public async Task<IList<ITimeSeriesItem>> GetJoinedList(int projectID, int nodeID, DateTime startDate, TimeComponent timeComponent, IList<TimeNaturalFlow> existingList, List<PastedTimeComponent> pasteList)
        {
            var startDateTime = startDate.GetTimeComponentValueCalculatedFromBeginingOfTheYear(timeComponent);
            var timeSeriesItemService = new TimeSeriesItemService();

            var fromPasteList = await timeSeriesItemService.GenerateTimeSeriesData(nodeID, startDate, timeComponent, pasteList, "TimeNaturalFlow", projectID);
            var fromExistingList = existingList.Where(x => x.Year < startDateTime.year || (x.Year == startDateTime.year && x.TimeComponentValue < startDateTime.timeComponentValue)).ToList();
            var combinedList = fromExistingList.Union(fromPasteList);

            return combinedList.ToList();
        }

        public async Task SaveAll(IList<TimeNaturalFlow> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task SaveAll(int projectID, int nodeID, DateTime startDate, TimeComponent timeComponent, IList<TimeNaturalFlow> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var timeComponentType = (int)timeComponent;
                var listToDelete = ctx.TimeNaturalFlows.Where(x => x.ProjectID == projectID)
                              .Where(x => x.NodeID == nodeID)
                              .Where(x => x.TimeComponentType == timeComponentType)
                              .ToList();

                await ctx.BulkDeleteAsync(listToDelete);
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<TimeNaturalFlow> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task Remove(IList<TimeNaturalFlow> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);            
            }
        }

        public async Task RemoveAll(int nodeID, TimeComponent timeComponent, int projectID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var listToRemove = (from tnf in ctx.TimeNaturalFlows
                                   where tnf.ProjectID == projectID
                                   where tnf.NodeID == nodeID
                                   where tnf.TimeComponentType == (int)timeComponent
                                   select tnf)
                                   .ToList();

                await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        public async Task SaveAllFromFile(int elementID, DateTime startDate, TimeComponent timeComponent, List<PastedTimeComponent> listFromFile, int projectID)
        {
            try
            {
                using (var ctx = new RBATContext(this._contextAccessor))
                {
                    var timeComponentType = (int)timeComponent;
                    var listToDelete = ctx.TimeNaturalFlows.Where(x => x.ProjectID == projectID)
                                  .Where(x => x.NodeID == elementID)
                                  .Where(x => x.TimeComponentType == timeComponentType)
                                  .ToList();

                    var timeSeriesItemService = new TimeSeriesItemService();
                    var timeSeriesItems = await timeSeriesItemService.GenerateTimeSeriesData(elementID, startDate, timeComponent, listFromFile, "TimeNaturalFlow", projectID);
                    List<TimeNaturalFlow> listToSave =  timeSeriesItems.OfType<TimeNaturalFlow>().ToList();

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
