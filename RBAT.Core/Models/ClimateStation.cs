using RBAT.Core.Clasess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Models
{
    public class ClimateStation : BaseEntityIntID
    {
        public ClimateStation() { }

        public ClimateStation(string name, string description, bool epType, int? id = null)
        {
            if (id != null) { Id = id.GetValueOrDefault(); };
            Name = name;
            Description = description;
            EpType = epType;
        }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        public bool EpType { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public int? Altitude { get; set; }

        public ICollection<NetEvaporation> NetEvaporations { get; set; }
    }    
}
