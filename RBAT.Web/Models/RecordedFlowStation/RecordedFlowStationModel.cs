using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Web.Models.RecordedFlowStation
{
    public class RecordedFlowStationModelList : List<RecordedFlowStationModel>
    {
    }

    public class RecordedFlowStationModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        public double? Latitude { get; set; }
        public string LatitudeString { get; set; }

        public double? Longitude { get; set; }
        public string LongitudeString { get; set; }

        public int? Altitude { get; set; }
        public string AltitudeString { get; set; }        
    }

    internal static partial class ViewModelExtensions
    {
        internal static RecordedFlowStationModel ToRecordedFlowStationModel(this Core.Models.RecordedFlowStation source)
        {
            if (source == null)
            {
                return null;
            }

            return new RecordedFlowStationModel
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                Latitude = source.Latitude,
                LatitudeString = (source.Latitude != null) ? source.Latitude.ToString() : "",
                Longitude = source.Longitude,
                LongitudeString = (source.Longitude != null) ? source.Longitude.ToString() : "",
                Altitude = source.Altitude,
                AltitudeString = (source.Altitude != null) ? source.Altitude.ToString() : ""
            };
        }

        internal static RecordedFlowStationModelList ToRecordedFlowStationViewModelList(this IEnumerable<Core.Models.RecordedFlowStation> source)
        {
            var rv = new RecordedFlowStationModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToRecordedFlowStationModel());
                }
            }

            return rv;
        }

        internal static Core.Models.RecordedFlowStation ToRecordedFlowStationEntityModel(this RecordedFlowStationModel source)
        {
            if (source == null)
            {
                return null;
            }

            return new Core.Models.RecordedFlowStation
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                Longitude = source.Longitude,
                Latitude = source.Latitude,
                Altitude = source.Altitude                
            };
        }
    }
}
