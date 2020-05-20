using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Web.Models.StartingReservoirLevel
{
    public class StartingReservoirLevelModelList : List<StartingReservoirLevelModel>
    {
    }

    public class StartingReservoirLevelModel
    {
        public long Id { get; set; }

        public int ProjectID { get; set; }

        public long ScenarioID { get; set; }
        public string ScenarioName { get; set; }

        public int NodeID { get; set; }
        public string NodeName { get; set; }

        public double InitialElevation { get; set; }
        public string InitialElevationText { get; set; }
    }

    internal static partial class ViewModelExtensions
    {
        internal static StartingReservoirLevelModel ToStartingReservoirLevelModel(this Core.Models.StartingReservoirLevel source)
        {
            if (source == null)
            {
                return null;
            }

            return new StartingReservoirLevelModel
            {
                Id = source.Id,
                ProjectID = source.Scenario.ProjectID,
                ScenarioID = source.ScenarioId,
                ScenarioName = source.Scenario.Name,
                NodeID = source.NodeId,
                NodeName = source.Node.Name,
                InitialElevation = source.InitialElevation,
                InitialElevationText = (source.InitialElevation == 0) ? "" : source.InitialElevation.ToString(),
            };
        }

        internal static StartingReservoirLevelModelList ToStartingReservoirLevelViewModelList(this IEnumerable<Core.Models.StartingReservoirLevel> source)
        {
            var rv = new StartingReservoirLevelModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToStartingReservoirLevelModel());
                }
            }

            return rv;
        }

        internal static Core.Models.StartingReservoirLevel ToStartingReservoirLevelEntityModel(this StartingReservoirLevelModel source)
        {
            if (source == null)
            {
                return null;
            }

            return new Core.Models.StartingReservoirLevel
            {
                Id = source.Id,
                ScenarioId = source.ScenarioID,
                NodeId = source.NodeID,
                InitialElevation = source.InitialElevation
            };
        }
    }
}
