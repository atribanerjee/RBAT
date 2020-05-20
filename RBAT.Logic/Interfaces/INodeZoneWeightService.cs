using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface INodeZoneWeightService
    {        
        Task<IList<NodeZoneWeight>> GetAll(int nodePolicyGroupID);
        Task<IList<NodeZoneWeight>> GetJoinedList(int nodePolicyGroupID, IList<NodeZoneWeight> existingList, List<ZoneLevelWithTimeComponent> pasteList);
        Task Save(NodeZoneWeight nodeZoneWeight);
        Task SaveAll(long nodePolicyGroupID, IList<NodeZoneWeight> listToSave);        
        Task RemoveAll(IList<NodeZoneWeight> listToRemove);
        Task UpdateAll(IList<NodeZoneWeight> listToUpdate);        
    }
}
