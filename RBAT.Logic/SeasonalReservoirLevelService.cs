using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using RBAT.Logic.TransferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{
    public class SeasonalReservoirLevelService : ISeasonalReservoirLevelService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SeasonalReservoirLevelService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<SeasonalReservoirLevel>> GetAll(SeasonalModel seasonalModel)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var seasonalReservoirLevel =  await ctx.StartingReservoirLevels
                                                       .Include(s => s.Node)
                                                       .Where(s => s.ScenarioId == seasonalModel.ScenarioId)
                                                       .Select(s => new SeasonalReservoirLevel
                                                       {
                                                           NodeId = s.NodeId,
                                                           NodeName = s.Node.Name
                                                       })
                                                       .ToListAsync();

                seasonalReservoirLevel.ForEach(n =>
                {
                    n.StorageLevel = Math.Round(seasonalModel.InitialElevation[seasonalReservoirLevel.IndexOf(n)], 3, MidpointRounding.AwayFromZero);
                });

                return seasonalReservoirLevel;
            }
        }

        public async Task<IList<SeasonalReservoirLevel>> UpdateExistingList(IList<SeasonalReservoirLevel> existingList, List<double> pasteList)
        {            
            await Task.Run(() => {
                foreach (var item in existingList.Select((value, i) => new { value, i }))
                {
                    item.value.StorageLevel = pasteList[item.i];
                }
            });

            return existingList;
        }
    }
}
