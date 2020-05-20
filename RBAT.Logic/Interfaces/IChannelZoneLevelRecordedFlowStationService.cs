using RBAT.Core.Models;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface IChannelZoneLevelRecordedFlowStationService
    {        
        Task<ChannelZoneLevelRecordedFlowStation> Get(int channelPolicyGroupId, int channelId);
        Task Save(ChannelZoneLevelRecordedFlowStation channelZoneLevelRecordedFlowStation);
    }
}
