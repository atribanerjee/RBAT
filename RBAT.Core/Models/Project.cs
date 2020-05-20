using RBAT.Core.Clasess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Models
{
    public class Project : BaseEntityIntID
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
        public DateTime CalculationBegins { get; set; }
        public DateTime CalculationEnds { get; set; }
        
        public int? RoutingOptionTypeID { get; set; }
        public RoutingOptionType RoutingOptionType { get; set; }

        public int? DataStepTypeID { get; set; }
        public TimeStepType DataStepType { get; set; }

        public string MapData { get; set; }

        public ICollection<TimeNaturalFlow> TimeNaturalFlows { get; set; }
        public ICollection<ProjectNode> ProjectNodes { get; set; }
        public ICollection<Scenario> Scenarios { get; set; }
        public ICollection<ProjectChannel> ProjectChannels { get; set; }
    }
}
