using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic.Extensions;
using RBAT.Logic.Interfaces;
using RBAT.Logic.TransferModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{
    public class ChannelService : IChannelService {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IChannelPolicyGroupChannelService _channelPolicyGroupChannelService;

        public ChannelService(IHttpContextAccessor contextAccessor, IChannelPolicyGroupChannelService channelPolicyGroupChannelService)
        {
            this._contextAccessor = contextAccessor;
            this._channelPolicyGroupChannelService = channelPolicyGroupChannelService;
        }

        public async Task<IList<Channel>> GetAll() {
            using ( var ctx = new RBATContext( this._contextAccessor ) ) {
                var channel = ctx.Channels
                                .Include(c => c.ChannelType)
                                .Include(c => c.UpstreamNode)
                                .Include(c => c.DownstreamNode)
                                .Include(c => c.UpstreamNodeWithControlStructure)
                                .Include(c => c.UpstreamChannelWithControlStructure)
                                .Include(c => c.RecordedFlowStation)
                                .Include(c => c.ReferenceNode)
                                .Include(c => c.UpstreamReservoirHeadWaterElevation)
                                .Include(c => c.UpstreamChannelHeadWaterElevation)
                                .Include(c => c.DownstreamReservoirTailWaterElevation)
                                .Include(c => c.DownstreamChannelTailWaterElevation)
                                .OrderBy(c => c.Name);                              

                return await channel.ToListAsync();
            }
        }

        public async Task<Channel> GetChannelByID(int channelId)
        {
            using (var ctx = new RBATContext( this._contextAccessor ))
            {
                return await ctx.Channels.FirstOrDefaultAsync(x => x.Id == channelId);
            }
        }

        public async Task Save(Channel channel)
        {
            using (var ctx = new RBATContext( this._contextAccessor ))
            {
                await ctx.Channels.AddAsync(channel);
                ctx.SaveChanges();
            }
        }       

        public async Task SaveAll(IList<Channel> listToSave)
        {
            using (var ctx = new RBATContext( ))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }       

        public async Task UpdateAll(IList<Channel> listToUpdate)
        {
            using (var ctx = new RBATContext( ))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveAll(IList<Channel> listToRemove)
        {
            using (var ctx = new RBATContext( ))
            {
                await UpdateChannelPolicyGroupChannelPriorities(listToRemove);
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        private async Task UpdateChannelPolicyGroupChannelPriorities(IList<Channel> channelsList) {
            if (channelsList != null)
            {
                using (var ctx = new RBATContext( this._contextAccessor ))
                {
                    var channelPolicyGroups = ctx.ChannelPolicyGroupChannels.Where(p => p.ChannelID == channelsList.First().Id).Select(p => p.ChannelPolicyGroup).ToList();

                    foreach (var g in channelPolicyGroups)
                    {
                        var priority = ctx.ChannelPolicyGroupChannels.Where(p => p.ChannelPolicyGroupID == g.Id)
                                                                     .Where(p => p.ChannelID == channelsList.First().Id)
                                                                     .Select(p => p.Priority)
                                                                     .First();

                        var channelPolicyGroupChannels = ctx.ChannelPolicyGroupChannels.Where(p => p.ChannelPolicyGroupID == g.Id)
                                                                                       .Where(p => p.Priority > priority);

                        if (channelPolicyGroupChannels.Any()) {
                            await channelPolicyGroupChannels.ForEachAsync(c => c.Priority = c.Priority - 1);
                            await _channelPolicyGroupChannelService.ChangePriority(await channelPolicyGroupChannels.ToListAsync());
                        }
                    };
                }
            }
        }

        public void UpdateChannelMapData(int channelId, string channelMapData)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelEntity = ctx.Channels.FirstOrDefault(x => x.Id == channelId);
                if (channelEntity != null)
                {
                    channelEntity.MapData = channelMapData;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
