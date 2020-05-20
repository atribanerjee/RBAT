using RBAT.Core.Clasess;
using System;

namespace RBAT.Core.Models
{
    public class TimeStorageCapacity : BaseEntityLongID
    {
        public TimeStorageCapacity() { }

        public TimeStorageCapacity(int nodeID, DateTime surveyDate, double elevation, double area, double volume)
        {
            NodeID = nodeID;
            SurveyDate = surveyDate;
            Elevation = elevation;
            Area = area;
            Volume = volume;            
        }

        public double Elevation { get; set; }
        public double Area { get; set; }
        public double Volume { get; set; }
        public DateTime SurveyDate { get; set; }

        public int NodeID { get; set; }
        public Node Node { get; set; }
    }
}
