using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Web.Models.ClimateStation
{
    public class ClimateStationModelList : List<ClimateStationModel>
    {
    }

    public class ClimateStationModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Description { get; set; }

        public bool EpType { get; set; }

        public double? Latitude { get; set; }
        public string LatitudeString { get; set; }

        public double? Longitude { get; set; }
        public string LongitudeString { get; set; }

        public int? Altitude { get; set; }
        public string AltitudeString { get; set; }        
    }

    internal static partial class ViewModelExtensions
    {
        internal static ClimateStationModel ToClimateStationModel(this Core.Models.ClimateStation source)
        {
            if (source == null)
            {
                return null;
            }

            return new ClimateStationModel
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                EpType = source.EpType,
                Latitude = source.Latitude,
                LatitudeString = (source.Latitude != null) ? source.Latitude.ToString() : "",
                Longitude = source.Longitude,
                LongitudeString = (source.Longitude != null) ? source.Longitude.ToString() : "",
                Altitude = source.Altitude,
                AltitudeString = (source.Altitude != null) ? source.Altitude.ToString() : ""
            };
        }

        internal static ClimateStationModelList ToClimateStationViewModelList(this IEnumerable<Core.Models.ClimateStation> source)
        {
            var rv = new ClimateStationModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToClimateStationModel());
                }
            }

            return rv;
        }

        internal static Core.Models.ClimateStation ToClimateStationEntityModel(this ClimateStationModel source)
        {
            if (source == null)
            {
                return null;
            }

            return new Core.Models.ClimateStation
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                EpType = source.EpType,
                Longitude = source.Longitude,
                Latitude = source.Latitude,
                Altitude = source.Altitude                
            };
        }
    }
}
