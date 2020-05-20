using System.Collections.Generic;
using System.Threading.Tasks;
using RBAT.Core.Models;

namespace RBAT.Logic.Interfaces {

    public interface INodeService {
        
        Task<Node> GetNodeByID(int nodeId);
        Task<IList<Node>> GetNodesByNodeType(int nodeTypeId);
        Task Save(Node node);
        Task SaveAll(IList<Node> listToSave);
        Task RemoveAll(IList<Node> listToRemove);
        Task UpdateAll(IList<Node> listToUpdate);
        void UpdateNodeMapData(int nodeId, string nodeMapData);
    }

}
