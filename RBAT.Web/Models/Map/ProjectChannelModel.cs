using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RBAT.Core.Models;

namespace RBAT.Web.Models.Map
{
    public class ProjectChannelModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? TypeId { get; set; }

        public string MapData { get; set; }
    }

    public class ProjectChannelModelList : List<ProjectChannelModel>
    {

    }

    internal static partial class ViewModelExtensions
    {

        internal static ProjectChannelModel ToProjectChannelModel(this ProjectChannel source)
        {
            if (source == null)
            {
                return null;
            }

            return new ProjectChannelModel
            {
                Id = source.ChannelId,
                Name = source.Channel?.Name,
                TypeId = source.Channel?.ChannelTypeId,
                MapData = source.Channel?.MapData
            };
        }

        internal static ProjectChannelModelList ToProjectChannelViewModelList(this IEnumerable<ProjectChannel> source)
        {
            var rv = new ProjectChannelModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToProjectChannelModel());
                }
            }

            return rv;
        }

    }
}
