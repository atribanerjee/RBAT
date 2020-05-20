using RBAT.Core.Clasess;
using RBAT.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace RBAT.Core.Models
{
    public class TimeNaturalFlow : TimeSeriesItem, ITimeSeriesItem
    {
        public TimeNaturalFlow() : base() { }

        public TimeNaturalFlow(int projectID, int nodeID, int year, int timeComponentType, 
                               int timeComponentValue, double? value) : 
            base(year, timeComponentType, timeComponentValue, value)
        {
            Value = value;
            ProjectID = projectID;
            NodeID = nodeID;
        }

        [Column("NaturalFlow")]
        public override double? Value { get; set; }

        public int ProjectID { get; set; }
        public Project Project { get; set; }

        public int NodeID { get; set; }
        public Node Node { get; set; }
    }
}
