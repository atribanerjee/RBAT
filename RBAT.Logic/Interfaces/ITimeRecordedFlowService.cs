using RBAT.Core.Clasess;
using RBAT.Core.Interfaces;
using RBAT.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface ITimeRecordedFlowService : IService, ITimeComponentService
    {
        Task<IList<TimeRecordedFlow>> GetAllTimeRecordedFlows(int recordedFlowStationID, TimeComponent timeComponent);
        Task<IList<TimeRecordedFlow>> GetTimeRecordedFlows(int recordedFlowStationID, int start, int length, TimeComponent timeComponent);
        Task<int> GetTimeRecordedFlowsTotalCount(int recordedFlowStationID, TimeComponent timeComponent);
        Task<IList<ITimeSeriesItem>> GetJoinedList(int recordedFlowStationID, DateTime startDate, TimeComponent timeComponent, IList<TimeRecordedFlow> existingList, List<PastedTimeComponent> pasteList);
        Task SaveAll(IList<TimeRecordedFlow> listToSAve);
        Task SaveAll(int recordedFlowStationID, DateTime startDate, TimeComponent timeComponent, IList<TimeRecordedFlow> listToSAve);
        Task Remove(IList<TimeRecordedFlow> listToRemove);
        Task RemoveAll(int recordedFlowStationID, TimeComponent timeComponent);
        Task UpdateAll(IList<TimeRecordedFlow> listToUpdate);        
    }
}
