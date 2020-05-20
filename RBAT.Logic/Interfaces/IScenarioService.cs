using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{

    public interface IScenarioService {        
        Task<Scenario> Get(long scenarioId);
        Task<Scenario> Get(long? scenarioId);
        Task<IList<Scenario>> GetAllByProjectID(int projectId);
        Task Save(Scenario scenario);
        Task SaveAll(IList<Scenario> listToSave);
        Task RemoveAll(IList<Scenario> listToRemove);
        Task UpdateAll(IList<Scenario> listToUpdate);
        Task<bool> CopyScenarios(int projectId, int newProjectId, Dictionary<int, int> nodes = null, Dictionary<int, int> channels = null, string username = null);
        Task<bool> CopyScenario(long scenarioId);
    }
}
