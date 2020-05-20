using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Web.Models.ChannelPolicyGroup
{
    public class ChannelPolicyGroupModelList : List<ChannelPolicyGroupModel>
    {
    }

    public class ChannelPolicyGroupModel
    {
        public long ScenarioID { get; set; }

        public long Id { get; set; }

        public int ChannelTypeID { get; set; }
        public string ChannelType { get; set; }

        public string Name { get; set; }

        [Required]
        public int NumberOfZonesAboveIdealLevel { get; set; }
        public string NumberOfZonesAbove { get; set; }

        [Required]
        public int NumberOfZonesBelowIdealLevel { get; set; }
        public string NumberOfZonesBelow { get; set; }

        public double? ZoneWeightsOffset { get; set; }
    }

    internal static partial class ViewModelExtensions
    {
        internal static ChannelPolicyGroupModel ToChannelPolicyGroupModel(this Core.Models.ChannelPolicyGroup source)
        {
            if (source == null)
            {
                return null;
            }

            return new ChannelPolicyGroupModel
            {
                ScenarioID = source.ScenarioID,
                Id = source.Id,
                ChannelTypeID = source.ChannelTypeID,
                ChannelType = source.ChannelType.Name,
                Name = source.Name,
                NumberOfZonesAboveIdealLevel = source.NumberOfZonesAboveIdealLevel,
                NumberOfZonesAbove = source.NumberOfZonesAboveIdealLevel.ToString(),
                NumberOfZonesBelowIdealLevel = source.NumberOfZonesBelowIdealLevel,
                NumberOfZonesBelow = source.NumberOfZonesBelowIdealLevel.ToString(),
                ZoneWeightsOffset = source.ZoneWeightsOffset
            };
        }

        internal static ChannelPolicyGroupModelList ToChannelPolicyGroupViewModelList(this IEnumerable<Core.Models.ChannelPolicyGroup> source)
        {
            var rv = new ChannelPolicyGroupModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToChannelPolicyGroupModel());
                }
            }

            return rv;
        }

        internal static Core.Models.ChannelPolicyGroup ToChannelPolicyGroupEntityModel(this ChannelPolicyGroupModel source)
        {
            if (source == null)
            {
                return null;
            }

            return new Core.Models.ChannelPolicyGroup
            {
                ScenarioID = source.ScenarioID,
                Id = source.Id,
                ChannelTypeID = source.ChannelTypeID,
                Name = source.Name,
                NumberOfZonesAboveIdealLevel = source.NumberOfZonesAboveIdealLevel,
                NumberOfZonesBelowIdealLevel = source.NumberOfZonesBelowIdealLevel,
                ZoneWeightsOffset = source.ZoneWeightsOffset
            };
        }
    }
}
