using RBAT.Core.Clasess;

namespace RBAT.Core.Models
{
    public class NetEvaporation: BaseEntityLongID
    {
        public NetEvaporation() { }

        public NetEvaporation(int nodeID, int climateStationID, double adjustmentFactor)
        {
            NodeID = nodeID;
            ClimateStationID = climateStationID;
            AdjustmentFactor = adjustmentFactor;
        }

        public double AdjustmentFactor { get; set; }

        public int NodeID { get; set; }
        public Node Node { get; set; }

        public int ClimateStationID { get; set; }
        public ClimateStation ClimateStation { get; set; }
    }
}
