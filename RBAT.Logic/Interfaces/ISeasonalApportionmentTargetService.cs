using RBAT.Core.Models;
using RBAT.Logic.TransferModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface ISeasonalApportionmentTargetService
    {
        Task<IList<SeasonalApportionmentTarget>> GetAll(SeasonalModel seasonalModel);
        Task<IList<SeasonalApportionmentTarget>> UpdateExistingList(IList<SeasonalApportionmentTarget> existingList, List<double> pasteList);        
    }
}