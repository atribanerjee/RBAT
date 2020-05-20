using RBAT.Core.Models;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface INodeZoneLevelHistoricReservoirLevelService
    {        
        Task<NodeZoneLevelHistoricReservoirLevel> Get(int nodePolicyGroupId, int nodeId);
        Task Save(NodeZoneLevelHistoricReservoirLevel nodeZoneLevelHistoricReservoirLevel);
    }
}