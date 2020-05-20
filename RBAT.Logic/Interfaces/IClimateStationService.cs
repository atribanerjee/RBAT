using System.Collections.Generic;
using System.Threading.Tasks;
using RBAT.Core.Models;

namespace RBAT.Logic.Interfaces {

    public interface IClimateStationService {        
        Task<IList<ClimateStation>> GetAll();
        Task<ClimateStation> GetClimateStationByID(int nodeId);
        Task Save(ClimateStation climateStation);
        Task RemoveAll(IList<ClimateStation> listToRemove);
        Task UpdateAll(IList<ClimateStation> listToUpdate);
    }

}
