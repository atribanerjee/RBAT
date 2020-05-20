using RBAT.Core.Clasess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RBAT.Core.Models
{
    public class Node : BaseEntityIntID
    {
        public Node() { }

        public Node(int nodeTypeID, string name, string description, int? id = null)
        {
            if (id != null) { Id = id.GetValueOrDefault(); };
            Name = name;
            Description = description;
            NodeTypeId = nodeTypeID;
        }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        public int NodeTypeId { get; set; }
        public NodeType NodeType { get; set; }
        
        public decimal? SizeOfIrrigatedArea { get; set; }
      
        public decimal? LandUseFactor { get; set; }

       // public bool? EqualDeficits { get; set; }

        public int? MeasuringUnitId { get; set; }
        public MeasuringUnit MeasuringUnit { get; set; }

        [Column(TypeName = "text")]
        public string MapData { get; set; }

        public ICollection<TimeWaterUse> TimeWaterUses { get; set; }
        public ICollection<TimeNaturalFlow> TimeNaturalFlows { get; set; }
        public ICollection<TimeHistoricLevel> TimeHistoricLevels { get; set; }
        public ICollection<NetEvaporation> NetEvaporations { get; set; }
        public ICollection<TimeStorageCapacity> TimeStorageCapacities { get; set; }
        public ICollection<ProjectNode> ProjectNodes { get; set; }
        public ICollection<NodePolicyGroupNode> NodePolicyGroupNodes { get; set; }
        public ICollection<NodeZoneLevel> NodeZoneLevels { get; set; }
    }
}
