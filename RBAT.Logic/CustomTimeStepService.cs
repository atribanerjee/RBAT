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
    public class CustomTimeStepService : ICustomTimeStepService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomTimeStepService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<List<CustomTimeStep>> GetCustomTimeSteps(long scenarioId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.CustomTimeSteps.Where(x => x.ScenarioId == scenarioId).ToListAsync();
            }
        }

        public async Task SaveProjectCustomTimeSteps(long scenarioId, IList<CustomTimeStep> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var listToDelete = ctx.CustomTimeSteps.Where(x => x.ScenarioId == scenarioId).ToList();

                await ctx.BulkDeleteAsync(listToDelete);
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task<IList<CustomTimeStep>> GetJoinedList(IList<CustomTimeStep> existingList, List<CustomTimeStep> pasteList)
        {
            var combinedList = existingList.Union(pasteList).ToAsyncEnumerable();
            return await combinedList.ToList();
        }

        public async Task UpdateCustomTimeStep(IList<CustomTimeStep> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveCustomTimeStep(IList<CustomTimeStep> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        public async Task SaveCustomTimeStep(long scenarioId, decimal timeStepValue)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var customTimeStep = new CustomTimeStep
                {
                    ScenarioId = scenarioId,
                    Length = timeStepValue
                };
                await ctx.CustomTimeSteps.AddAsync(customTimeStep);
                ctx.SaveChanges();
            }
        }
    }
}
