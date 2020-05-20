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
    public class SeasonalApportionmentTargetService : ISeasonalApportionmentTargetService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SeasonalApportionmentTargetService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<SeasonalApportionmentTarget>> GetAll(SeasonalModel seasonalModel)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var seasonalApportionmentTarget =  await ctx.ProjectChannels
                                                            .Include(c => c.Channel)
                                                            .Where(c => c.ProjectId == seasonalModel.ProjectId)
                                                            .Where(c => c.Channel.ChannelTypeId == 1) //River reach Channel
                                                            .Where(c => c.Channel.ApportionmentFlowTarget != null)
                                                            .Select(c => new SeasonalApportionmentTarget
                                                            {
                                                                ChannelId = c.ChannelId,
                                                                ChannelName =  c.Channel.Name                                                                
                                                            })
                                                            .ToListAsync();

                seasonalApportionmentTarget.ForEach(c =>
                {
                    c.ApportionmentFlowVolume = Math.Round(seasonalModel.ApportionmentAgreements[seasonalApportionmentTarget.IndexOf(c)], 3, MidpointRounding.AwayFromZero);
                });

                return seasonalApportionmentTarget;
            }
        }

        public async Task<IList<SeasonalApportionmentTarget>> UpdateExistingList(IList<SeasonalApportionmentTarget> existingList, List<double> pasteList)
        {            
            await Task.Run(() => {
                foreach (var item in existingList.Select((value, i) => new { value, i }))
                {
                    item.value.ApportionmentFlowVolume = pasteList[item.i];
                }
            });

            return existingList;
        }
    }
}
