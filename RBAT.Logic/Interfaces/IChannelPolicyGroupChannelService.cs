using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface IChannelPolicyGroupChannelService
    {
        Task<IList<Channel>> Get(int channelPolicyGroupID);
        Task ChangePriority(IList<ChannelPolicyGroupChannel> listToUpdate);
    }
}