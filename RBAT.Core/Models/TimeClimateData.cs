using RBAT.Core.Clasess;
using RBAT.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace RBAT.Core.Models
{
    public class TimeClimateData : TimeSeriesItem, ITimeSeriesItem
    {
        public TimeClimateData() : base() { }

        public TimeClimateData(int climateStationID, int year, int timeComponentType, int timeComponentValue, double? value) :
            base(year, timeComponentType, timeComponentValue, value)
        {
            Value = value;
            ClimateStationID = climateStationID;
        }

        [Column("ClimateData")]
        public override double? Value { get; set; }

        public int ClimateStationID { get; set; }
        public ClimateStation ClimateStation { get; set; }
    }
}
