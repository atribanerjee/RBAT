using RBAT.Core.Clasess;
using RBAT.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace RBAT.Core.Models
{
    public class TimeRecordedFlow : TimeSeriesItem, ITimeSeriesItem
    {
        public TimeRecordedFlow() : base() { }

        public TimeRecordedFlow(int recordedFlowStationID, int year, int timeComponentType,
                               int timeComponentValue, double? value) :
            base(year, timeComponentType, timeComponentValue, value)
        {
            Value = value;
            RecordedFlowStationID = recordedFlowStationID;
        }

        [Column("RecordedFlow")]
        public override double? Value { get; set; }

        public int RecordedFlowStationID { get; set; }
        public RecordedFlowStation RecordedFlowStation { get; set; }       
    }
}
