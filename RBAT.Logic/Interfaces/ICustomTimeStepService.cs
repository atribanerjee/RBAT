using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface ICustomTimeStepService
    {
        Task<List<CustomTimeStep>> GetCustomTimeSteps(long scenarioId);
        Task SaveProjectCustomTimeSteps(long scenarioId, IList<CustomTimeStep> listToSave);
        Task<IList<CustomTimeStep>> GetJoinedList(IList<CustomTimeStep> existingList, List<CustomTimeStep> pasteList);
        Task UpdateCustomTimeStep(IList<CustomTimeStep> listToUpdate);
        Task RemoveCustomTimeStep(IList<CustomTimeStep> listToRemove);
        Task SaveCustomTimeStep(long scenarioId, decimal timeStepValue);
    }
}
