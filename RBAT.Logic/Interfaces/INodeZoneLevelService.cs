using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface INodeZoneLevelService
    {        
        Task<IList<NodeZoneLevel>> GetAll(int nodePolicyGroupID, int nodeID);
        Task<IList<NodeZoneLevel>> GetJoinedList(int nodePolicyGroupID, int nodeID, IList<NodeZoneLevel> existingList, List<ZoneLevelWithTimeComponent> pasteList);
        Task SaveAll(int nodePolicyGroupID, int nodeID, IList<NodeZoneLevel> listToSave);        
        Task RemoveAll(IList<NodeZoneLevel> listToRemove);
        Task UpdateAll(IList<NodeZoneLevel> listToUpdate);        
    }
}
