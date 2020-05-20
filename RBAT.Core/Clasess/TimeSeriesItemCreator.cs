using RBAT.Core.Interfaces;
using System;

namespace RBAT.Core.Clasess
{
    public abstract class TimeSeriesItemCreator
    {
        public abstract ITimeSeriesItem GetItem(int nodeID, (int timeComponentValue, int year) startDate, TimeComponent timeComponent, double? itemValue, string type, int? projectID = null);
    }
}
