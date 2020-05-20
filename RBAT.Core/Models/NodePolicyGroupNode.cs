using RBAT.Core.Clasess;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Models
{
    public class NodePolicyGroupNode :  BaseEntityLongID
    {
        [Required]
        public long NodePolicyGroupID { get; set; }
        public NodePolicyGroup NodePolicyGroup { get; set; }

        [Required]
        public int NodeID { get; set; }
        public Node Node { get; set; }

        [Required]
        public int Priority { get; set; }
    }
}
