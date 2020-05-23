using RBAT.Core.Clasess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Models
{
    public class Scenario : BaseEntityLongID
    {
        [Required]
        public int ProjectID { get; set; }
        public Project Project { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int? DataTimeStepTypeID { get; set; }
        public TimeStepType DataTimeStepType { get; set; }

        public int? TimeStepTypeID { get; set; }
        public TimeStepType TimeStepType { get; set; }

        public DateTime? CalculationBegins { get; set; }
        public DateTime? CalculationEnds { get; set; }

        public int? ScenarioTypeID { get; set; }
        public ScenarioType ScenarioType { get; set; }

        [DefaultValue(0)]
        public int NumberOfLookaheadTimeSteps { get; set; }
        public ICollection<ChannelPolicyGroup> ChannelPolicyGroups { get; set; }
        public ICollection<NodePolicyGroup> NodePolicyGroups { get; set; }
        public ICollection<CustomTimeStep> CustomTimeSteps { get; set; }
        public ICollection<StartingReservoirLevel> StartingReservoirLevels { get; set; }

        [DefaultValue(1)]
        public bool OutflowConstraintsUsed { get; set; }
    }
}
