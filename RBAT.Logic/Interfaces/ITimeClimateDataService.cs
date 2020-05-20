using RBAT.Core.Clasess;
using RBAT.Core.Interfaces;
using RBAT.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace RBAT.Logic.Interfaces
{
    public interface ITimeClimateDataService : ITimeComponentService
    {
        Task<IList<TimeClimateData>> GetAllTimeClimateData(int climateStationID, TimeComponent timeComponent);
        Task<IList<TimeClimateData>> GetTimeClimateData(int climateStationID, int start, int length, TimeComponent timeComponent);
        Task<int> GetTimeClimateDatalTotalCount(int climateStationID, TimeComponent timeComponent);
        Task<IList<ITimeSeriesItem>> GetJoinedList(int climateStationID, DateTime startDate, TimeComponent timeComponent, IList<TimeClimateData> existingList, List<PastedTimeComponent> pasteList);
        Task SaveAll(IList<TimeClimateData> listToSAve);
        Task SaveAll(int climateStationID, DateTime startDate, TimeComponent timeComponent, IList<TimeClimateData> listToSave);
        Task Remove(IList<TimeClimateData> listToRemove);
        Task RemoveAll(int climateStationID, TimeComponent timeComponent);
        Task UpdateAll(IList<TimeClimateData> listToUpdate);
    }
}
