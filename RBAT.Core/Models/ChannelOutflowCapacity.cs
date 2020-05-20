using RBAT.Core.Clasess;

namespace RBAT.Core.Models
{
    public class ChannelOutflowCapacity : BaseEntityLongID
    {
        public ChannelOutflowCapacity() { }

        public ChannelOutflowCapacity(int channelID, double elevation, double maximumOutflow)
        {
            ChannelID = channelID;
            Elevation = elevation;
            MaximumOutflow = maximumOutflow;
        }

        public double Elevation { get; set; }
        public double MaximumOutflow { get; set; }

        public Channel Channel { get; set; }
        public int ChannelID { get; set; }        
    }
}
