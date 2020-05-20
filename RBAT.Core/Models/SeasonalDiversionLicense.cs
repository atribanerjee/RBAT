namespace RBAT.Core.Models
{
    public class SeasonalDiversionLicense
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public double WaterLicenseVolume { get; set; }
        public double? MaximalRateDiversionLicenses { get; set; }
    }
}
