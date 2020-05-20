using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Web.Models.Scenario
{
    public class ScenarioModelList : List<ScenarioModel>
    {
    }

    public class ScenarioModel
    {
        public int ProjectID { get; set; }

        public long Id { get; set; }

        [StringLength(250)]
        [Required]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int? DataTimeStepTypeID { get; set; }
        public string DataTimeStepType { get; set; }

        public int? TimeStepTypeID { get; set; }
        public string TimeStepType { get; set; }

        public string CalculationBegins { get; set; }
        public DateTime? CalculationBeginsDate { get; set; }

        public string CalculationEnds { get; set; }
        public DateTime? CalculationEndsDate { get; set; }

        public int? ScenarioTypeID { get; set; }
        public string ScenarioType { get; set; }

        public string NumberOfLookaheadTimeSteps { get; set; }
    }

    internal static partial class ViewModelExtensions
    {
        internal static ScenarioModel ToScenarioModel(this Core.Models.Scenario source)
        {
            if (source == null)
            {
                return null;
            }

            return new ScenarioModel
            {
                ProjectID = source.ProjectID,
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                CalculationBegins = source.CalculationBegins.GetValueOrDefault().ToString("d", System.Globalization.CultureInfo.InvariantCulture),
                CalculationBeginsDate = source.CalculationBegins,
                CalculationEnds = source.CalculationEnds.GetValueOrDefault().ToString("d", System.Globalization.CultureInfo.InvariantCulture),
                CalculationEndsDate = source.CalculationEnds,
                DataTimeStepTypeID = source.DataTimeStepTypeID,
                DataTimeStepType = source.DataTimeStepType?.Name,
                TimeStepTypeID = source.TimeStepTypeID,
                TimeStepType = source.TimeStepType?.Name,
                ScenarioTypeID = source.ScenarioTypeID,
                ScenarioType = source.ScenarioType?.Name,
                NumberOfLookaheadTimeSteps = source.NumberOfLookaheadTimeSteps.ToString()
            };
        }

        internal static ScenarioModelList ToScenarioViewModelList(this IEnumerable<Core.Models.Scenario> source)
        {
            var rv = new ScenarioModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToScenarioModel());
                }
            }

            return rv;
        }

        internal static Core.Models.Scenario ToScenarioEntityModel(this ScenarioModel source)
        {
            if (source == null)
            {
                return null;
            }

            return new Core.Models.Scenario
            {
                ProjectID = source.ProjectID,
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                CalculationBegins = source.CalculationBeginsDate,
                CalculationEnds = source.CalculationEndsDate,
                TimeStepTypeID = source.TimeStepTypeID,
                DataTimeStepTypeID = source.DataTimeStepTypeID,
                ScenarioTypeID = source.ScenarioTypeID,
                NumberOfLookaheadTimeSteps = source.NumberOfLookaheadTimeSteps == string.Empty ? 0  : Convert.ToInt16(source.NumberOfLookaheadTimeSteps)
            };
        }
    }
}
