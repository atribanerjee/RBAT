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
    public class StartingReservoirLevelService : IStartingReservoirLevelService {

        private readonly IHttpContextAccessor _contextAccessor;

        public StartingReservoirLevelService(IHttpContextAccessor contextAccessor) {
            _contextAccessor = contextAccessor;
        }

        public async Task<IList<StartingReservoirLevel>> GetAllByProjectID(int projectId)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var startingReservoirLevel = ctx.StartingReservoirLevels
                                  .Include(s => s.Scenario)
                                  .Include(s => s.Node)
                                  .Where(s => s.Scenario.ProjectID == projectId)
                                  .OrderBy(s => s.Scenario.Name)
                                  .ThenBy(s => s.Node.Name);

                return await startingReservoirLevel.ToListAsync();
            }
        }

        public async Task<StartingReservoirLevel> Get(int startingReservoirLevelId)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                return await ctx.StartingReservoirLevels.Include(s => s.Scenario).Include(s => s.Node)
                                                        .FirstOrDefaultAsync(x => x.Id == startingReservoirLevelId);
            }
        }

        //public async Task<StartingReservoirLevel> Get(int? startingReservoirLevelId)
        //{
        //    using (var ctx = new RBATContext(_contextAccessor))
        //    {
        //        if (startingReservoirLevelId == null) {
        //            var dropDownService = new DropDownService(_contextAccessor);
        //            var projects = dropDownService.ListProjects();
        //            if (projects != null)
        //                return await ctx.StartingReservoirLevels.FirstOrDefaultAsync(x => x.ProjectID == projects.FirstOrDefault().Id);
        //            else
        //                return null;
        //        }
        //        else
        //            return await ctx.StartingReservoirLevels.FirstOrDefaultAsync(x => x.Id == startingReservoirLevelId.GetValueOrDefault());

        //    }
        //}

        public async Task Save(StartingReservoirLevel startingReservoirLevel)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                await ctx.StartingReservoirLevels.AddAsync(startingReservoirLevel);
                ctx.SaveChanges();
            }
        }

        public async Task SaveAll(IList<StartingReservoirLevel> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<StartingReservoirLevel> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveAll(IList<StartingReservoirLevel> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }       
    }
}
