using RBAT.Core.Interfaces;
using RBAT.Core.Models;
using System;

namespace RBAT.Core.Clasess
{
    public class ConcreteTimeSeriesItemCreator : TimeSeriesItemCreator
    {
        public override ITimeSeriesItem GetItem(int elementID, (int timeComponentValue, int year) startDate, TimeComponent timeComponent, double? itemValue, string type, int? projectID)
        {
            switch (type)
            {
                case "TimeNaturalFlow":
                    return new TimeNaturalFlow(projectID.GetValueOrDefault(), elementID, startDate.year, (int)timeComponent, startDate.timeComponentValue, itemValue);
                case "TimeWaterUse":
                    return new TimeWaterUse(elementID, startDate.year, (int)timeComponent, startDate.timeComponentValue, itemValue);
                case "TimeHistoricLevel":
                    return new TimeHistoricLevel(elementID, startDate.year, (int)timeComponent, startDate.timeComponentValue, itemValue);
                case "TimeClimateData":
                    return new TimeClimateData(elementID, startDate.year, (int)timeComponent, startDate.timeComponentValue, itemValue);
                case "TimeRecordedFlow":
                    return new TimeRecordedFlow(elementID, startDate.year, (int)timeComponent, startDate.timeComponentValue, itemValue);
                default: throw new ArgumentException("Invalid type", "type");
            }
        }
    }
}
