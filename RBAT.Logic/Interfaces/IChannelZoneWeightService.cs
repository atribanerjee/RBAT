using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface IChannelZoneWeightService
    {        
        Task<IList<ChannelZoneWeight>> GetAll(int channelPolicyGroupID);
        Task<IList<ChannelZoneWeight>> GetJoinedList(int channelPolicyGroupID, IList<ChannelZoneWeight> existingList, List<ZoneLevelWithTimeComponent> pasteList);
        Task Save(ChannelZoneWeight channelZoneWeight);
        Task SaveAll(long channelPolicyGroupID, IList<ChannelZoneWeight> listToSave);        
        Task RemoveAll(IList<ChannelZoneWeight> listToRemove);
        Task UpdateAll(IList<ChannelZoneWeight> listToUpdate);        
    }
}
