using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface ITimeStorageCapacityService
    {
        Task<IList<TimeStorageCapacity>> GetAll(int nodeID);
        Task<IList<TimeStorageCapacity>> GetJoinedList(IList<TimeStorageCapacity> existingList, List<TimeStorageCapacity> pasteList);
        Task SaveAll(IList<TimeStorageCapacity> listToSave);
        Task SaveAll(int nodeID, IList<TimeStorageCapacity> listToSave);
        Task RemoveAll(IList<TimeStorageCapacity> listToRemove);
        Task UpdateAll(IList<TimeStorageCapacity> listToUpdate);
    }
}
