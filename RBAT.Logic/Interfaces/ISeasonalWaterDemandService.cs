using RBAT.Logic.TransferModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface ISeasonalWaterDemandService
    {
        Task<(IList<string>, IList<IList<double>>)> GetAll(SeasonalModel seasonalModel);
    }
}
