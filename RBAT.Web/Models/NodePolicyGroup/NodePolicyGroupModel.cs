using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Web.Models.NodePolicyGroup
{
    public class NodePolicyGroupModelList : List<NodePolicyGroupModel>
    {
    }

    public class NodePolicyGroupModel
    {
        public long ScenarioID { get; set; }

        public long Id { get; set; }

        public int NodeTypeID { get; set; }
        public string NodeType { get; set; }

        public string Name { get; set; }

        [Required]
        public int NumberOfZonesAboveIdealLevel { get; set; }
        public string NumberOfZonesAbove { get; set; }

        [Required]
        public int NumberOfZonesBelowIdealLevel { get; set; }
        public string NumberOfZonesBelow { get; set; }

        public double? ZoneWeightsOffset { get; set; }

        public bool EqualDeficits { get; set; }

        public bool CopyNodeLevelasFirstComponent { get; set; }

        public int? StartTimeStep { get; set; }

        public int? EndTimeStep { get; set; }

    }

    internal static partial class ViewModelExtensions
    {
        internal static NodePolicyGroupModel ToNodePolicyGroupModel(this Core.Models.NodePolicyGroup source)
        {
            if (source == null)
            {
                return null;
            }

            return new NodePolicyGroupModel
            {
                ScenarioID = source.ScenarioID,
                Id = source.Id,
                NodeTypeID = source.NodeTypeID,
                NodeType = source.NodeType.Name,
                Name = source.Name,
                NumberOfZonesAboveIdealLevel = source.NumberOfZonesAboveIdealLevel,
                NumberOfZonesAbove = source.NumberOfZonesAboveIdealLevel.ToString(),
                NumberOfZonesBelowIdealLevel = source.NumberOfZonesBelowIdealLevel,
                NumberOfZonesBelow = source.NumberOfZonesBelowIdealLevel.ToString(),
                ZoneWeightsOffset = source.ZoneWeightsOffset,
                EqualDeficits = source.EqualDeficits,
                CopyNodeLevelasFirstComponent = source.CopyNodeLevelasFirstComponent,
                StartTimeStep = source.StartTimeStep,
                EndTimeStep = source.EndTimeStep
            };
        }

        internal static NodePolicyGroupModelList ToNodePolicyGroupViewModelList(this IEnumerable<Core.Models.NodePolicyGroup> source)
        {
            var rv = new NodePolicyGroupModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToNodePolicyGroupModel());
                }
            }

            return rv;
        }

        internal static Core.Models.NodePolicyGroup ToNodePolicyGroupEntityModel(this NodePolicyGroupModel source)
        {
            if (source == null)
            {
                return null;
            }

            return new Core.Models.NodePolicyGroup
            {
                ScenarioID = source.ScenarioID,
                Id = source.Id,
                NodeTypeID = source.NodeTypeID,
                Name = source.Name,
                NumberOfZonesAboveIdealLevel = source.NumberOfZonesAboveIdealLevel,
                NumberOfZonesBelowIdealLevel = source.NumberOfZonesBelowIdealLevel,
                ZoneWeightsOffset = source.ZoneWeightsOffset,
                EqualDeficits = source.EqualDeficits,
                CopyNodeLevelasFirstComponent = source.CopyNodeLevelasFirstComponent,
                StartTimeStep = source.StartTimeStep,
                EndTimeStep = source.EndTimeStep
            };
        }
    }
}
