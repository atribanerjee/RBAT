using RBAT.Core.Clasess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Models
{
    public class NodePolicyGroup : BaseEntityLongID
    {
        [Required]
        public long ScenarioID { get; set; }
        public Scenario Scenario { get; set; }

        [Required]
        public int NodeTypeID { get; set; }
        public NodeType NodeType { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int NumberOfZonesAboveIdealLevel { get; set; }

        [Required]
        public int NumberOfZonesBelowIdealLevel { get; set; }

        public double? ZoneWeightsOffset { get; set; }

        public bool EqualDeficits { get; set; }

        public bool CopyNodeLevelasFirstComponent { get; set; }

        public int? StartTimeStep { get; set; }

        public int? EndTimeStep { get; set; }

        public ICollection<NodeZoneLevel> NodeZoneLevels { get; set; }
        public ICollection<NodeZoneWeight> NodeZoneWeights { get; set; }
        public ICollection<NodePolicyGroupNode> NodePolicyGroupNodes { get; set; }
    }
}
