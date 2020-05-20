using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RBAT.Core.Models;

namespace RBAT.Web.Models.Project
{
    public class ProjectChannelModelList : List<ProjectChannelModel>
    {

    }
    public class ProjectChannelModel
    {
        public bool IsSelected { get; set; }

        public int ChannelId { get; set; }

        public string ChannelName { get; set; }

        public string ChannelType { get; set; }

        public int ProjectId { get; set; }

    }

    internal static partial class ViewModelExtensions
    {

        internal static ProjectChannelModel ToProjectChannelModel(this ProjectChannel source, int projectId)
        {
            if (source == null)
            {
                return null;
            }

            return new ProjectChannelModel
            {
                ChannelId = source.ChannelId,
                ProjectId = source.ProjectId,
                ChannelName = source.Channel?.Name,
                ChannelType = source.Channel?.ChannelType?.Name,
                IsSelected = source.ProjectId == projectId
            };
        }

        internal static ProjectChannelModelList ToProjectChannelViewModelList(this IEnumerable<ProjectChannel> source, int projectId)
        {
            var rv = new ProjectChannelModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToProjectChannelModel(projectId));
                }
            }

            return rv;
        }

    }

}