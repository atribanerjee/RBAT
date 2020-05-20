using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RBAT.Core;
using RBAT.Logic.Interfaces;
using RBAT.Logic.TransferModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{
    public class SeasonalWaterDemandService : ISeasonalWaterDemandService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SeasonalWaterDemandService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<(IList<string>, IList<IList<double>>)> GetAll(SeasonalModel seasonalModel)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var nodeList = await ctx.ProjectNodes.Include(p => p.Node)
                                                     .Where(p => p.ProjectId == seasonalModel.ProjectId)
                                                     .Where(p => p.Node.NodeType.Id == 2) //Consumptive Use
                                                     .Select(s => s.Node.Name)
                                                     .ToListAsync();

                var list = new List<IList<double>>();
                for (int i = 0; i < seasonalModel.WaterDemands[0].Count; i++)
                {
                    var tempList = new List<double>();
                    for (int j = 0; j < seasonalModel.WaterDemands.Count; j++)
                    {
                        tempList.Add(Math.Round(seasonalModel.WaterDemands[j][i], 3, MidpointRounding.AwayFromZero));
                    }

                    list.Add(tempList);
                }               

                return (nodeList, list);
            }              
        }        
    }
}
