using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Web.Models.Project
{
    public class ProjectModelList : List<ProjectModel>
    {
    }

    public class ProjectModel
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(250)]
        [Required]
        public string Description { get; set; }

        [Required]
        public int? DataReadTypeId { get; set; }

        public string DataReadType { get; set; }

        [Required]
        public int? RoutingOptionTypeId { get; set; }

        public string RoutingOptionType { get; set; }

        public string CalculationBegin { get; set; }

        public DateTime CalculationBeginDate { get; set; }

        public string CalculationEnd { get; set; }

        public DateTime CalculationEndDate { get; set; }
    }

    internal static partial class ViewModelExtensions
    {

        internal static ProjectModel ToProjectModel(this Core.Models.Project source)
        {
            if (source == null)
            {
                return null;
            }

            return new ProjectModel
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                CalculationBegin = source.CalculationBegins.ToString("d", System.Globalization.CultureInfo.InvariantCulture),
                CalculationBeginDate = source.CalculationBegins,
                CalculationEnd = source.CalculationEnds.ToString("d", System.Globalization.CultureInfo.InvariantCulture),
                CalculationEndDate = source.CalculationEnds,
                DataReadTypeId = source.DataStepTypeID,
                DataReadType = source.DataStepType?.Name,
                RoutingOptionTypeId = source.RoutingOptionTypeID,
                RoutingOptionType = source.RoutingOptionType?.Name
            };
        }

        internal static ProjectModelList ToProjectViewModelList(this IEnumerable<Core.Models.Project> source)
        {
            var rv = new ProjectModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToProjectModel());
                }
            }

            return rv;
        }

        internal static Core.Models.Project ToProjectEntityModel(this RBAT.Web.Models.Project.ProjectModel source)
        {
            if (source == null)
            {
                return null;
            }

            return new Core.Models.Project
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                CalculationBegins = source.CalculationBeginDate,
                CalculationEnds = source.CalculationEndDate,
                DataStepTypeID = source.DataReadTypeId,
                RoutingOptionTypeID = source.RoutingOptionTypeId
            };
        }

    }
}
