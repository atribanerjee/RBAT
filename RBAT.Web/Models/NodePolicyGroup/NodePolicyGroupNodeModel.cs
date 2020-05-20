using RBAT.Core.Models;
using System.Collections.Generic;

namespace RBAT.Web.Models.NodePolicyGroup
{
    public class NodePolicyGroupNodeModelList : List<NodePolicyGroupNodeModel>
    {

    }
    public class NodePolicyGroupNodeModel
    {
        public bool IsSelected { get; set; }

        public int NodeID { get; set; }

        public string NodeName { get; set; }

        public string NodeType { get; set; }

        public long NodePolicyGroupID { get; set; }

    }

    internal static partial class ViewModelExtensions
    {

        internal static NodePolicyGroupNodeModel ToNodePolicyGroupNodeModel(this NodePolicyGroupNode source, int nodePolicyGroupID)
        {
            if (source == null)
            {
                return null;
            }

            return new NodePolicyGroupNodeModel
            {
                NodeID = source.NodeID,
                NodePolicyGroupID = source.NodePolicyGroupID,
                NodeName = source.Node?.Name,
                NodeType = source.Node?.NodeType?.Name,
                IsSelected = source.NodePolicyGroupID == nodePolicyGroupID
            };
        }

        internal static NodePolicyGroupNodeModelList ToNodePolicyGroupNodeViewModelList(this IEnumerable<NodePolicyGroupNode> source, int projectId)
        {
            var rv = new NodePolicyGroupNodeModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToNodePolicyGroupNodeModel(projectId));
                }
            }

            return rv;
        }

    }

}