using RBAT.Core.Clasess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Models
{
    public class RecordedFlowStation : BaseEntityIntID
    {
        public RecordedFlowStation() { }

        public RecordedFlowStation(string name, string description, int? id = null)
        {
            if (id != null) { Id = id.GetValueOrDefault(); };
            Name = name;
            Description = description;
        }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public int? Altitude { get; set; }

        public ICollection<TimeRecordedFlow> TimeRecordedFlows { get; set; }
        public ICollection<Channel> Channel { get; set; }
    }
}
