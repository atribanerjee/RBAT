using RBAT.Core.Models;
using RBAT.Logic.TransferModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface ISeasonalReservoirLevelService
    {
        Task<IList<SeasonalReservoirLevel>> GetAll(SeasonalModel seasonalModel);
        Task<IList<SeasonalReservoirLevel>> UpdateExistingList(IList<SeasonalReservoirLevel> existingList, List<double> pasteList);        
    }
}
