namespace RBAT.Logic.TransferModels
{
    public class NetEvaporationModel
    {
        public long Id { get; set; }
        public int NodeId { get; set; }
        public int ClimateStationId { get; set; }
        public string ClimateStationName { get; set; }
        public double AdjustmentFactor { get; set; }
    }
}
