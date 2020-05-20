using RBAT.Core.Models;
using RBAT.Logic.TransferModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface IChannelService {                
        Task<IList<Channel>> GetAll();
        Task<Channel> GetChannelByID(int channelId);
        Task Save(Channel channel);
        Task SaveAll(IList<Channel> listToSave);        
        Task RemoveAll(IList<Channel> listToRemove);
        Task UpdateAll(IList<Channel> listToUpdate);
        void UpdateChannelMapData(int channelId, string channelMapData);
    }
}
