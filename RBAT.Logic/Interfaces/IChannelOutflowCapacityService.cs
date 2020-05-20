using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface IChannelOutflowCapacityService
    {
        Task<IList<ChannelOutflowCapacity>> GetAll(int channelID);
        Task<IList<ChannelOutflowCapacity>> GetJoinedList(IList<ChannelOutflowCapacity> existingList, List<ChannelOutflowCapacity> pasteList);
        Task SaveAll(IList<ChannelOutflowCapacity> listToSave);
        Task SaveAll(int channelID, IList<ChannelOutflowCapacity> listToSave);
        Task RemoveAll(IList<ChannelOutflowCapacity> listToRemove);
        Task UpdateAll(IList<ChannelOutflowCapacity> listToUpdate);
    }
}
