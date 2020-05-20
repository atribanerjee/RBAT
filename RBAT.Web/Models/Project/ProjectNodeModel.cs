using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RBAT.Core.Models;

namespace RBAT.Web.Models.Project {

    public class ProjectNodeModelList : List<ProjectNodeModel> {

    }

    public class ProjectNodeModel {

        public bool IsSelected { get; set; }

        public int NodeId { get; set; }

        public string NodeName { get; set; }

        public string NodeType { get; set; }

        public int ProjectId { get; set; }

    }

    internal static partial class ViewModelExtensions {

        internal static ProjectNodeModel ToProjectNodeModel( this ProjectNode source, int projectId ) {
            if ( source == null ) {
                return null;
            }

            return new ProjectNodeModel {
                NodeId = source.NodeId,
                ProjectId = source.ProjectId,
                NodeName = source.Node?.Name,
                NodeType = source.Node?.NodeType?.Name,
                IsSelected = source.ProjectId == projectId
            };
        }

        internal static ProjectNodeModelList ToProjectNodeViewModelList( this IEnumerable<ProjectNode> source, int projectId ) {
            var rv = new ProjectNodeModelList();
            if ( source != null ) {
                foreach ( var item in source ) {
                    rv.Add( item.ToProjectNodeModel( projectId ) );
                }
            }

            return rv;
        }

    }

}
