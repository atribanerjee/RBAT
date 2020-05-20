using RBAT.Core.Clasess;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Models
{
    public class NodeZoneLevelHistoricReservoirLevel : BaseEntityLongID
    {
        [Required]
        public long NodePolicyGroupId { get; set; }
        public NodePolicyGroup NodePolicyGroup { get; set; }

        [Required]
        public int NodeId { get; set; }
        public Node Node { get; set; }

        public bool UseHistoricReservoirLevels { get; set; }        
    }
}

