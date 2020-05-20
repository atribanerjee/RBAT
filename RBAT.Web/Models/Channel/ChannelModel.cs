using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Web.Models.Channel
{
    public class ChannelModelList : List<ChannelModel>
    {
    }

    public class ChannelModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        public int ChannelTypeId { get; set; }
        public string ChannelTypeName { get; set; }

        public double? RoutingCoefficientA { get; set; }
        public string RoutingCoefficientAText { get; set; }
        public double? RoutingCoefficientN { get; set; }
        public string RoutingCoefficientNText { get; set; }

        [Range(0.0, 1.0), Display(Name = "ReturnFlowFraction")]
        public double? PercentReturnFlow { get; set; }
        public string PercentReturnFlowText { get; set; }

        public int? NumberOfRoutingPhases { get; set; }
        public string NumberOfRoutingPhasesText { get; set; }

        public bool RoutingOptionUse { get; set; }

        public string UpstreamNodeName { get; set; }
        public string DownstreamNodeName { get; set; }
        public string UpstreamNodeWithControlStructureName { get; set; }
        public string UpstreamChannelWithControlStructureName { get; set; }
        public string RecordedFlowStationName { get; set; }

        public int? UpstreamNodeID { get; set; }
        public int? DownstreamNodeID { get; set; }
        public int? UpstreamNodeWithControlStructureID { get; set; }
        public int? UpstreamChannelWithControlStructureID { get; set; }
        public int? RecordedFlowStationID { get; set; }

        [Range(0, 1000000000.0000000)]
        public double? TotalLicensedVolume { get; set; }
        public string TotalLicensedVolumeText { get; set; }

        [Range(0, 1.0000000)]
        public double? ApportionmentFlowTarget { get; set; }
        public string ApportionmentFlowTargetText { get; set; }

        public string ReferenceNodeName { get; set; }
        public int? ReferenceNodeID { get; set; }

        public double? OverallHydroPowerPlantEfficiency { get; set; }
        public double? RatedPower { get; set; }
        public string OverallHydroPowerPlantEfficiencyText { get; set; }
        public double? ConstantHeadWaterLevel { get; set; }
        public string ConstantHeadWaterLevelText { get; set; }
        public string UpstreamReservoirHeadWaterElevationName { get; set; }
        public int? UpstreamReservoirHeadWaterElevationID { get; set; }
        public string UpstreamChannelHeadWaterElevationName { get; set; }
        public int? UpstreamChannelHeadWaterElevationID { get; set; }
        public double? ConstantTailWaterLevel { get; set; }
        public string ConstantTailWaterLevelText { get; set; }
        public string DownstreamReservoirTailWaterElevationName { get; set; }
        public int? DownstreamReservoirTailWaterElevationID { get; set; }
        public string DownstreamChannelTailWaterElevationName { get; set; }
        public int? DownstreamChannelTailWaterElevationID { get; set; }

        public string MapData { get; set; }
    }

    internal static partial class ViewModelExtensions
    {
        internal static ChannelModel ToChannelModel(this Core.Models.Channel source)
        {
            if (source == null)
            {
                return null;
            }

            return new ChannelModel
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                ChannelTypeId = source.ChannelTypeId,
                ChannelTypeName = source.ChannelType?.Name ?? "",
                RoutingCoefficientA = source.RoutingCoefficientA,
                RoutingCoefficientAText = (source.RoutingCoefficientA != null) ? source.RoutingCoefficientA.ToString() : "",
                RoutingCoefficientN = source.RoutingCoefficientN,
                RoutingCoefficientNText = (source.RoutingCoefficientN != null) ? source.RoutingCoefficientN.ToString() : "",
                PercentReturnFlow = source.PercentReturnFlow,
                PercentReturnFlowText = (source.PercentReturnFlow != null) ? source.PercentReturnFlow.ToString() : "",
                NumberOfRoutingPhases = source.NumberOfRoutingPhases,
                NumberOfRoutingPhasesText = (source.NumberOfRoutingPhases != null) ? source.NumberOfRoutingPhases.ToString() : "",
                RoutingOptionUse = source.RoutingOptionUse,
                UpstreamNodeName = source.UpstreamNode?.Name ?? "",
                DownstreamNodeName = source.DownstreamNode?.Name ?? "",
                UpstreamNodeWithControlStructureName = source.UpstreamNodeWithControlStructure?.Name ?? "",
                UpstreamChannelWithControlStructureName = source.UpstreamChannelWithControlStructure?.Name ?? "",
                RecordedFlowStationName = source.RecordedFlowStation?.Name ?? "",
                UpstreamNodeID = source.UpstreamNodeID,
                DownstreamNodeID = source.DownstreamNodeID,
                UpstreamNodeWithControlStructureID = source.UpstreamNodeWithControlStructureID,
                UpstreamChannelWithControlStructureID = source.UpstreamChannelWithControlStructureID,
                RecordedFlowStationID = source.RecordedFlowStationID,
                TotalLicensedVolume = source.TotalLicensedVolume,
                TotalLicensedVolumeText = (source.TotalLicensedVolume != null) ? source.TotalLicensedVolume.ToString() : "",
                ApportionmentFlowTarget = source.ApportionmentFlowTarget,
                ApportionmentFlowTargetText = (source.ApportionmentFlowTarget != null) ? source.ApportionmentFlowTarget.ToString() : "",
                MapData = source.MapData,
                ReferenceNodeName = source.ReferenceNode?.Name ?? "",
                ReferenceNodeID = source.ReferenceNodeID,
                OverallHydroPowerPlantEfficiency = source.OverallHydroPowerPlantEfficiency,
                OverallHydroPowerPlantEfficiencyText = (source.OverallHydroPowerPlantEfficiency != null) ? source.OverallHydroPowerPlantEfficiency.ToString() : "",
                RatedPower = source.RatedPower,
                ConstantHeadWaterLevel = source.ConstantHeadWaterLevel,
                ConstantHeadWaterLevelText = (source.ConstantHeadWaterLevel != null) ? source.ConstantHeadWaterLevel.ToString() : "",
                UpstreamReservoirHeadWaterElevationName = source.UpstreamReservoirHeadWaterElevation?.Name ?? "",
                UpstreamReservoirHeadWaterElevationID = source.UpstreamReservoirHeadWaterElevationID,
                UpstreamChannelHeadWaterElevationName = source.UpstreamChannelHeadWaterElevation?.Name ?? "",
                UpstreamChannelHeadWaterElevationID = source.UpstreamChannelHeadWaterElevationID,
                ConstantTailWaterLevel = source.ConstantTailWaterLevel,
                ConstantTailWaterLevelText = (source.ConstantTailWaterLevel != null) ? source.ConstantTailWaterLevel.ToString() : "",
                DownstreamReservoirTailWaterElevationName = source.DownstreamReservoirTailWaterElevation?.Name ?? "",
                DownstreamReservoirTailWaterElevationID = source.DownstreamReservoirTailWaterElevationID,
                DownstreamChannelTailWaterElevationName = source.DownstreamChannelTailWaterElevation?.Name ?? "",
                DownstreamChannelTailWaterElevationID = source.DownstreamChannelTailWaterElevationID
            };
        }

        internal static ChannelModelList ToChannelViewModelList(this IEnumerable<Core.Models.Channel> source)
        {
            var rv = new ChannelModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToChannelModel());
                }
            }

            return rv;
        }

        internal static Core.Models.Channel ToChannelEntityModel(this ChannelModel source)
        {
            if (source == null)
            {
                return null;
            }

            return new Core.Models.Channel
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                ChannelTypeId = source.ChannelTypeId,
                RoutingCoefficientA = source.RoutingCoefficientA,
                RoutingCoefficientN = source.RoutingCoefficientN,
                PercentReturnFlow = source.PercentReturnFlow,
                NumberOfRoutingPhases = source.NumberOfRoutingPhases,
                RoutingOptionUse = source.RoutingOptionUse,
                UpstreamNodeID = source.UpstreamNodeID,
                DownstreamNodeID = source.DownstreamNodeID,
                UpstreamNodeWithControlStructureID = source.UpstreamNodeWithControlStructureID,
                UpstreamChannelWithControlStructureID = source.UpstreamChannelWithControlStructureID,
                RecordedFlowStationID = source.RecordedFlowStationID,
                TotalLicensedVolume = source.TotalLicensedVolume,
                ApportionmentFlowTarget = source.ApportionmentFlowTarget,
                MapData = source.MapData,
                ReferenceNodeID = source.ReferenceNodeID,
                OverallHydroPowerPlantEfficiency = source.OverallHydroPowerPlantEfficiency,
                ConstantHeadWaterLevel = source.ConstantHeadWaterLevel,
                UpstreamReservoirHeadWaterElevationID = source.UpstreamReservoirHeadWaterElevationID,
                RatedPower = source.RatedPower,
                UpstreamChannelHeadWaterElevationID = source.UpstreamChannelHeadWaterElevationID,
                ConstantTailWaterLevel = source.ConstantTailWaterLevel,
                DownstreamReservoirTailWaterElevationID = source.DownstreamReservoirTailWaterElevationID,
                DownstreamChannelTailWaterElevationID = source.DownstreamChannelTailWaterElevationID
            };
        }
    }
}
