using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface IChannelPolicyGroupService
    {        
        Task<ChannelPolicyGroup> Get(int channelPolicyGroupID);
        Task<IList<ChannelPolicyGroup>> GetAllByScenarioID(int scenarioId);
        Task Save(ChannelPolicyGroup channelPolicyGroup);
        Task SaveAll(IList<ChannelPolicyGroup> listToSave);
        Task RemoveAll(IList<ChannelPolicyGroup> listToRemove);
        Task UpdateAll(IList<ChannelPolicyGroup> listToUpdate);
        Task<IList<ChannelPolicyGroupChannel>> GetAllChannels(int channelPolicyGroupId);
        Task<bool> SaveChannelPolicyGroupChannel(int channelPolicyGroupID, int channelId, bool isChecked);
        bool CheckChannelPolicyGroupNode(int channelPolicyGroupID, int channelId);
    }
}
