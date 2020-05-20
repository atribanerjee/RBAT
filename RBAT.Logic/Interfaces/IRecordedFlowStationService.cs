using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface IRecordedFlowStationService
    {
        Task<RecordedFlowStation> GetRecordedFlowStationByID(int id);
        Task<IList<RecordedFlowStation>> GetAll();
        Task Save(RecordedFlowStation recordedFlowStation);
        Task SaveAll(IList<RecordedFlowStation> listToSave);
        Task RemoveAll(IList<RecordedFlowStation> listToRemove);
        Task UpdateAll(IList<RecordedFlowStation> listToUpdate);
    }
}
