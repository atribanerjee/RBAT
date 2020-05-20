using RBAT.Core.Models;
using RBAT.Logic.TransferModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface ISeasonalDiversionLicenseService
    {
        Task<IList<SeasonalDiversionLicense>> GetAll(SeasonalModel seasonalModel);
        Task<IList<SeasonalDiversionLicense>> UpdateExistingList(IList<SeasonalDiversionLicense> existingList, List<SeasonalDiversionLicense> pasteList);        
    }
}
