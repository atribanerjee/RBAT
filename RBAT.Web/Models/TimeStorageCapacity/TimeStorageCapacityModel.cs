using System;
using System.Collections.Generic;

namespace RBAT.Web.Models.TimeStorageCapacity
{
    public class TimeStorageCapacityModelViewModelList : List<TimeStorageCapacityModel> {

    }

    public class TimeStorageCapacityModel {

        public long Id { get; set; }
        public double Elevation { get; set; }
        public double Area { get; set; }
        public double Volume { get; set; }
        public string SurveyDate { get; set; }
        public int NodeID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }

    internal static class ViewModelExtensions
    {
        internal static TimeStorageCapacityModel ToTimeStorageCapacityModel(this Core.Models.TimeStorageCapacity source)
        {
            if (source == null)
            {
                return null;
            }

            return new TimeStorageCapacityModel
            {
                Id = source.Id,
                NodeID = source.NodeID,
                Elevation = source.Elevation,
                Area = source.Area,
                Volume = source.Volume,
                SurveyDate = source.SurveyDate.ToShortDateString(),
                Created = source.Created,
                Modified = source.Modified
            };
        }

        internal static TimeStorageCapacityModelViewModelList ToTimeStorageCapacityViewModelList(this IEnumerable<Core.Models.TimeStorageCapacity> source)
        {
            var rv = new TimeStorageCapacityModelViewModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToTimeStorageCapacityModel());
                }
            }

            return rv;
        }
    }
}
