using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface INodePolicyGroupNodeService
    {
        Task<IList<Node>> Get(int nodePolicyGroupID);
        Task ChangePriority(IList<NodePolicyGroupNode> listToUpdate);
    }
}