using RBAT.Core.Models;
using RBAT.Logic.TransferModels;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RBAT.Logic.Interfaces
{
    public interface INetEvaporationService
    {
        Task<IList<NetEvaporationModel>> GetAll(int nodeID);        
        Task SaveAll(IList<NetEvaporation> listToSave);
        Task RemoveAll(IList<NetEvaporation> listToRemove);
        Task Update(NetEvaporation item);
    }
}
