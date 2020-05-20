using RBAT.Core.Models;
using System.Collections.Generic;

namespace RBAT.Web.Models.ChannelPolicyGroup
{
    public class ChannelPolicyGroupChannelModelList : List<ChannelPolicyGroupChannelModel>
    {

    }
    public class ChannelPolicyGroupChannelModel
    {
        public bool IsSelected { get; set; }

        public int ChannelID { get; set; }

        public string ChannelName { get; set; }

        public string ChannelType { get; set; }

        public long ChannelPolicyGroupID { get; set; }

    }

    internal static partial class ViewModelExtensions
    {

        internal static ChannelPolicyGroupChannelModel ToChannelPolicyGroupChannelModel(this ChannelPolicyGroupChannel source, int channelPolicyGroupID)
        {
            if (source == null)
            {
                return null;
            }

            return new ChannelPolicyGroupChannelModel
            {
                ChannelID = source.ChannelID,
                ChannelPolicyGroupID = source.ChannelPolicyGroupID,
                ChannelName = source.Channel?.Name,
                ChannelType = source.Channel?.ChannelType?.Name,
                IsSelected = source.ChannelPolicyGroupID == channelPolicyGroupID
            };
        }

        internal static ChannelPolicyGroupChannelModelList ToChannelPolicyGroupChannelViewModelList(this IEnumerable<ChannelPolicyGroupChannel> source, int projectId)
        {
            var rv = new ChannelPolicyGroupChannelModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToChannelPolicyGroupChannelModel(projectId));
                }
            }

            return rv;
        }

    }

}