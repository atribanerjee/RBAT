using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RBAT.Core.Models;

namespace RBAT.Web.Models.Map
{
    public class ProjectNodeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? TypeId { get; set; }

        public string MapData { get; set; }
    }

    public class ProjectNodeModelList : List<ProjectNodeModel>
    {

    }

    internal static partial class ViewModelExtensions
    {

        internal static ProjectNodeModel ToProjectNodeModel(this ProjectNode source)
        {
            if (source == null)
            {
                return null;
            }

            return new ProjectNodeModel
            {
                Id = source.NodeId,
                Name = source.Node?.Name,
                TypeId = source.Node?.NodeTypeId,
                MapData = source.Node?.MapData
            };
        }

        internal static ProjectNodeModelList ToProjectNodeViewModelList(this IEnumerable<ProjectNode> source)
        {
            var rv = new ProjectNodeModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToProjectNodeModel());
                }
            }

            return rv;
        }

    }
}
