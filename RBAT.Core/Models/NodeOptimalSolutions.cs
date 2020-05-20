using System;
using System.Collections.Generic;
using System.Text;
using RBAT.Core.Clasess;

namespace RBAT.Core.Models
{
    public class NodeOptimalSolutions : BaseEntityLongID
    {
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public long ScenarioID { get; set; }
        public Scenario Scenario { get; set; }
        public string ScenarioName { get; set; }
        public int NodeID { get; set; }
        public Node Node { get; set; }
        public bool IsNetEvaporation { get; set; }
        public DateTime? SimulationStartDate { get; set; }
        public DateTime? SimulationEndDate { get; set; }
        public bool UseUnitsOfVolume { get; set; }
    }
}
