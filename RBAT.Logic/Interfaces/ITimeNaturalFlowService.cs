using RBAT.Core.Clasess;
using RBAT.Core.Interfaces;
using RBAT.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface ITimeNaturalFlowService : IService, ITimeComponentService
    {
        Task<IList<TimeNaturalFlow>> GetAllTimeNaturalFlows(int nodeID, TimeComponent timeComponent, int projectID);
        Task<IList<TimeNaturalFlow>> GetTimeNaturalFlows(int projectID, int nodeID, int start, int length, TimeComponent timeComponent);
        Task<int> GetTimeNaturalFlowsTotalCount(int projectID, int nodeID, TimeComponent timeComponent);
        Task<IList<ITimeSeriesItem>> GetJoinedList(int projectID, int nodeID, DateTime startDate, TimeComponent timeComponent, IList<TimeNaturalFlow> existingList, List<PastedTimeComponent> pasteList);
        Task SaveAll(IList<TimeNaturalFlow> listToSAve);
        Task SaveAll(int projectID, int nodeID, DateTime startDate, TimeComponent timeComponent, IList<TimeNaturalFlow> listToSAve);
        Task Remove(IList<TimeNaturalFlow> listToRemove);
        Task RemoveAll(int nodeID, TimeComponent timeComponent, int projectID);
        Task UpdateAll(IList<TimeNaturalFlow> listToUpdate);
    }
}
