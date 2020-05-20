using RBAT.Core.Clasess;
using RBAT.Core.Interfaces;
using RBAT.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface ITimeHistoricLevelService : ITimeComponentService
    {
        Task<IList<TimeHistoricLevel>> GetAllTimeHistoricLevels(int nodeID, TimeComponent timeComponent = TimeComponent.Month);
        Task<IList<TimeHistoricLevel>> GetTimeHistoricLevel(int nodeID, int start, int length, TimeComponent timeComponent = TimeComponent.Month);
        Task<int> GetTimeHistoricLevelTotalCount(int nodeID, TimeComponent timeComponent = TimeComponent.Month);
        Task<IList<ITimeSeriesItem>> GetJoinedList(int nodeID, DateTime startDate, TimeComponent timeComponent, IList<TimeHistoricLevel> existingList, List<PastedTimeComponent> pasteList);
        Task SaveAll(IList<TimeHistoricLevel> listToSAve);
        Task SaveAll(int nodeID, DateTime startDate, TimeComponent timeComponent, IList<TimeHistoricLevel> listToSave);
        Task Remove(IList<TimeHistoricLevel> listToRemove);
        Task RemoveAll(int nodeID, TimeComponent timeComponent = TimeComponent.Month);
        Task UpdateAll(IList<TimeHistoricLevel> listToUpdate);
    }
}
