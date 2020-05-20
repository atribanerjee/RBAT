using RBAT.Core.Clasess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Models
{
    public class ChannelPolicyGroup : BaseEntityLongID
    {
        [Required]
        public long ScenarioID { get; set; }
        public Scenario Scenario { get; set; }

        [Required]
        public int ChannelTypeID { get; set; }
        public ChannelType ChannelType { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int NumberOfZonesAboveIdealLevel { get; set; }

        [Required]
        public int NumberOfZonesBelowIdealLevel { get; set; }

        public double? ZoneWeightsOffset { get; set; }

        public ICollection<ChannelZoneLevel> ChannelZoneLevels { get; set; }
        public ICollection<ChannelZoneWeight> ChannelZoneWeights { get; set; }
        public ICollection<ChannelPolicyGroupChannel> ChannelPolicyGroupChannels { get; set; }
        public ICollection<ChannelZoneLevelRecordedFlowStation> ChannelZoneLevelRecordedFlowStations { get; set; }
    }
}
