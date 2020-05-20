using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Web.Models.Node
{
    public class NodeModelList : List<NodeModel>
    {
    }

    public class NodeModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        public int NodeTypeId { get; set; }
        public string NodeType { get; set; }
        
        public decimal? SizeOfIrrigatedArea { get; set; }
      
        public decimal? LandUseFactor { get; set; }

        public bool EqualDeficits { get; set; }

        public int? MeasuringUnitId { get; set; }
        public string MeasuringUnit { get; set; }
    }

    internal static partial class ViewModelExtensions
    {
        internal static NodeModel ToNodeModel(this Core.Models.Node source)
        {
            if (source == null)
            {
                return null;
            }

            return new NodeModel
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                NodeTypeId = source.NodeTypeId,
                NodeType = source.NodeType?.Name ?? "",
                SizeOfIrrigatedArea = source.SizeOfIrrigatedArea,
                LandUseFactor = source.LandUseFactor,
                //EqualDeficits = source.EqualDeficits.GetValueOrDefault(),
                MeasuringUnitId = source.MeasuringUnitId,
                MeasuringUnit = source.MeasuringUnit?.Name ?? ""                
            };
        }

        internal static NodeModelList ToNodeViewModelList(this IEnumerable<Core.Models.Node> source)
        {
            var rv = new NodeModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToNodeModel());
                }
            }

            return rv;
        }

        internal static Core.Models.Node ToNodeEntityModel(this NodeModel source)
        {
            if (source == null)
            {
                return null;
            }

            return new Core.Models.Node
            {                
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                NodeTypeId = source.NodeTypeId,
                SizeOfIrrigatedArea = source.SizeOfIrrigatedArea,
                LandUseFactor = source.LandUseFactor,
                //EqualDeficits = (source.NodeTypeId == 2) ? (bool?)source.EqualDeficits : null,
                MeasuringUnitId = source.MeasuringUnitId
            };
        }
    }
}
