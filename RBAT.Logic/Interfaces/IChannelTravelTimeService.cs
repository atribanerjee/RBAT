using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface IChannelTravelTimeService
    {
        Task<IList<ChannelTravelTime>> GetAll(int channelID);
        Task<IList<ChannelTravelTime>> GetJoinedList(IList<ChannelTravelTime> existingList, List<ChannelTravelTime> pasteList);
        Task SaveAll(IList<ChannelTravelTime> listToSave);
        Task SaveAll(int channelID, IList<ChannelTravelTime> listToSave);
        Task RemoveAll(IList<ChannelTravelTime> listToRemove);
        Task UpdateAll(IList<ChannelTravelTime> listToUpdate);
    }
}
