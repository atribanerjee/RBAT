using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{

    public interface IStartingReservoirLevelService {        
        Task<StartingReservoirLevel> Get(int startingReservoirLevelId);
        Task<IList<StartingReservoirLevel>> GetAllByProjectID(int projectId);
        Task Save(StartingReservoirLevel startingReservoirLevel);
        Task SaveAll(IList<StartingReservoirLevel> listToSave);
        Task RemoveAll(IList<StartingReservoirLevel> listToRemove);
        Task UpdateAll(IList<StartingReservoirLevel> listToUpdate);
    }
}
