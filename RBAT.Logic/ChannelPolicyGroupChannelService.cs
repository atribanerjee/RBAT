using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{
    public class ChannelPolicyGroupChannelService : IChannelPolicyGroupChannelService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ChannelPolicyGroupChannelService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<IList<Channel>> Get(int channelPolicyGroupID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.ChannelPolicyGroupChannels
                                .Include(c => c.Channel)
                                .Where(c => c.ChannelPolicyGroupID == channelPolicyGroupID)
                                .OrderBy(c => c.Priority)
                                .Select(c => c.Channel)                                
                                .ToListAsync();
            }
        }

        public async Task ChangePriority(IList<ChannelPolicyGroupChannel> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var config = new BulkConfig
                {
                    PropertiesToInclude = new List<string> {
                        nameof(ChannelPolicyGroupChannel.Priority)
                    },
                    UpdateByProperties = new List<string> {
                        nameof(ChannelPolicyGroupChannel.ChannelPolicyGroupID),
                        nameof(ChannelPolicyGroupChannel.ChannelID)
                    }
                };
                await ctx.BulkUpdateAsync(listToUpdate, config);
            }
        }
    }    
}
