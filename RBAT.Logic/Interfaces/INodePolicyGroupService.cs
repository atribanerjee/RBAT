using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface INodePolicyGroupService
    {        
        Task<NodePolicyGroup> Get(int nodePolicyGroupID);
        Task<IList<NodePolicyGroup>> GetAllByScenarioID(int scenarioId);
        Task Save(NodePolicyGroup nodePolicyGroup);
        Task SaveAll(IList<NodePolicyGroup> listToSave);
        Task RemoveAll(IList<NodePolicyGroup> listToRemove);
        Task UpdateAll(IList<NodePolicyGroup> listToUpdate);
        Task<IList<NodePolicyGroupNode>> GetAllNodes(int nodePolicyGroupId);
        Task<bool> SaveNodePolicyGroupNode(int nodePolicyGroupID, int nodeId, bool isChecked);
        bool CheckNodePolicyGroupNode(int nodePolicyGroupID, int nodeId);
    }
}
