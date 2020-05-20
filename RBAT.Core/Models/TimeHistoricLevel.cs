using RBAT.Core.Clasess;
using RBAT.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace RBAT.Core.Models
{
    public class TimeHistoricLevel : TimeSeriesItem, ITimeSeriesItem
    {
        public TimeHistoricLevel() : base() { }

        public TimeHistoricLevel(int nodeID, int year, int timeComponentType, int timeComponentValue, double? value) :
            base(year, timeComponentType, timeComponentValue, value)
        {
            Value = value;
            NodeID = nodeID;
        }

        [Column("Elevation")]
        public override double? Value { get; set; }

        public int NodeID { get; set; }
        public Node Node { get; set; }
    }
}
