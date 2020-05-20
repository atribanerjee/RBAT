using RBAT.Core.Clasess;
using RBAT.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace RBAT.Core.Models
{
    public class TimeWaterUse : TimeSeriesItem, ITimeSeriesItem
    {
        public TimeWaterUse() : base() { }

        public TimeWaterUse(int nodeID, int year, int timeComponentType, int timeComponentValue, double? value) :
            base(year, timeComponentType, timeComponentValue, value)
        {
            Value = value;
            NodeID = nodeID;
        }

        [Column("WaterUse")]
        public override double? Value { get; set; }

        public int NodeID { get; set; }
        public Node Node { get; set; }
    }
}
