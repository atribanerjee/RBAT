using RBAT.Core.Clasess;

namespace RBAT.Core.Models
{
    public class ChannelTravelTime : BaseEntityLongID
    {
        public ChannelTravelTime() { }

        public ChannelTravelTime(int channelID, double flow, double travelTime)
        {
            ChannelID = channelID;
            Flow = flow;
            TravelTime = travelTime;
        }

        public double Flow { get; set; }
        public double TravelTime { get; set; }

        public Channel Channel { get; set; }
        public int ChannelID { get; set; }        
    }
}
