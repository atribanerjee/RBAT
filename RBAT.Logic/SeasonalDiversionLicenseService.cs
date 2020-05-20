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
    public class SeasonalDiversionLicenseService : ISeasonalDiversionLicenseService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SeasonalDiversionLicenseService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        public async Task<IList<SeasonalDiversionLicense>> GetAll(SeasonalModel seasonalModel)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var seasonalDiversionLicenses = await ctx.ProjectChannels
                                                         .Include(c => c.Channel)
                                                         .Where(c => c.ProjectId == seasonalModel.ProjectId)
                                                         .Where(c => c.Channel.ChannelTypeId == 2) //Diversion Channel
                                                         .Where(c => c.Channel.TotalLicensedVolume != null)
                                                         .Select(c => new SeasonalDiversionLicense
                                                         {
                                                             ChannelId = c.ChannelId,
                                                             ChannelName =  c.Channel.Name,                                            
                                                         })
                                                         .ToListAsync();

                seasonalDiversionLicenses.ForEach(c =>
                {
                    c.WaterLicenseVolume = Math.Round(seasonalModel.CumulativeVolumeDiversionLicenses[seasonalDiversionLicenses.IndexOf(c)], 3, MidpointRounding.AwayFromZero);
                });

                return seasonalDiversionLicenses;
            }
        }

        public async Task<IList<SeasonalDiversionLicense>> UpdateExistingList(IList<SeasonalDiversionLicense> existingList, List<SeasonalDiversionLicense> pasteList)
        {            
            await Task.Run(() => {
                foreach (var item in existingList.Select((value, i) => new { value, i }))
                {
                    item.value.WaterLicenseVolume = pasteList[item.i].WaterLicenseVolume;
                    item.value.MaximalRateDiversionLicenses = pasteList[item.i].MaximalRateDiversionLicenses;
                }
            });

            return existingList;
        }
    }
}
