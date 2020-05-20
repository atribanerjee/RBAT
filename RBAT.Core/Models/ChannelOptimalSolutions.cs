using RBAT.Core.Clasess;
using System;

namespace RBAT.Core.Models
{
    public class ChannelOptimalSolutions : BaseEntityLongID
    {
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public long ScenarioID { get; set; }
        public Scenario Scenario { get; set; }
        public string ScenarioName { get; set; }
        public int ChannelID { get; set; }
        public Channel Channel { get; set; }
        public DateTime? SimulationStartDate { get; set; }
        public DateTime? SimulationEndDate { get; set; }
        public bool UseUnitsOfVolume { get; set; }
}
}
