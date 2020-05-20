using RBAT.Core.Clasess;
using RBAT.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RBAT.Core.Interfaces;

namespace RBAT.Logic.Interfaces
{
    public interface ITimeWaterUseService : ITimeComponentService
    {
        Task<IList<TimeWaterUse>> GetAllTimeWaterUse(int nodeID, TimeComponent timeComponent);
        Task<IList<TimeWaterUse>> GetTimeWaterUse(int nodeID, int start, int length, TimeComponent timeComponent);
        Task<int> GetTimeWaterUseTotalCount(int nodeID, TimeComponent timeComponent);
        Task<IList<ITimeSeriesItem>> GetJoinedList(int nodeID, DateTime startDate, TimeComponent timeComponent, IList<TimeWaterUse> existingList, List<PastedTimeComponent> pasteList);
        Task SaveAll(IList<TimeWaterUse> listToSAve);
        Task SaveAll(int nodeID, DateTime startDate, TimeComponent timeComponent, IList<TimeWaterUse> listToSave);
        Task Remove(IList<TimeWaterUse> listToRemove);
        Task RemoveAll(int nodeID, TimeComponent timeComponent);
        Task UpdateAll(IList<TimeWaterUse> listToUpdate);
    }
}
