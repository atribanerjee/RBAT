using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface IChannelZoneLevelService
    {        
        Task<IList<ChannelZoneLevel>> GetAll(int channelPolicyGroupID, int channelID);
        Task<IList<ChannelZoneLevel>> GetJoinedList(int channelPolicyGroupID, int channelID, IList<ChannelZoneLevel> existingList, List<ZoneLevelWithTimeComponent> pasteList);
        Task SaveAll(int channelPolicyGroupID, int channelID, IList<ChannelZoneLevel> listToSave);        
        Task RemoveAll(IList<ChannelZoneLevel> listToRemove);
        Task UpdateAll(IList<ChannelZoneLevel> listToUpdate);        
    }
}
