using RBAT.Core.Clasess;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RBAT.Core.Models
{
    public class Channel : BaseEntityIntID
    {
        public Channel() { }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        public int ChannelTypeId { get; set; }
        public ChannelType ChannelType { get; set; }

       
        public double? RoutingCoefficientA {get; set;}
        public double? RoutingCoefficientN { get; set; }

        [Range(0.0, 1.0)]
        public double? PercentReturnFlow { get; set; }

        public int? NumberOfRoutingPhases { get; set; }
        public bool RoutingOptionUse { get; set; }

        public Node UpstreamNode { get; set; }        
        public Node DownstreamNode { get; set; }
        public Node UpstreamNodeWithControlStructure { get; set; }        
        public RecordedFlowStation RecordedFlowStation { get; set; }

        public int? UpstreamNodeID { get; set; }
        public int? DownstreamNodeID { get; set; }        
        public int? UpstreamNodeWithControlStructureID { get; set; }

        public int? UpstreamChannelWithControlStructureID { get; set; }
        public Channel UpstreamChannelWithControlStructure { get; set; }

        public int? RecordedFlowStationID { get; set; }

        [Range(0,1000000000.0000000)]
        public double? TotalLicensedVolume { get; set; }

        [Range(0, 1.0000000)]
        public double? ApportionmentFlowTarget { get; set; }
        
        public Node ReferenceNode { get; set; }
        public int? ReferenceNodeID { get; set; }        

        public double? OverallHydroPowerPlantEfficiency { get; set; }
        public double? RatedPower { get; set; }
        public double? ConstantHeadWaterLevel { get; set; }
        public Node UpstreamReservoirHeadWaterElevation { get; set; }
        public int? UpstreamReservoirHeadWaterElevationID { get; set; }

        public int? UpstreamChannelHeadWaterElevationID { get; set; }
        public Channel UpstreamChannelHeadWaterElevation { get; set; }

        public double? ConstantTailWaterLevel { get; set; }
        public Node DownstreamReservoirTailWaterElevation { get; set; }
        public int? DownstreamReservoirTailWaterElevationID { get; set; }

        public int? DownstreamChannelTailWaterElevationID { get; set; }
        public Channel DownstreamChannelTailWaterElevation { get; set; }

        [Column(TypeName = "text")]
        public string MapData { get; set; }

        public ICollection<ChannelTravelTime> TravelTimes { get; set; }
        public ICollection<ChannelOutflowCapacity> ChannelOutflowCapacities { get; set; }
        public ICollection<ChannelZoneLevel> ChannelZoneLevels { get; set; }
        public ICollection<ProjectChannel> ProjectChannels { get; set; }
        public ICollection<ChannelPolicyGroupChannel> ChannelPolicyGroupChannels { get; set; }
    }
}
