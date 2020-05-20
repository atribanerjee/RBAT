using System;
using System.Collections.Generic;

namespace RBAT.Logic.TransferModels
{
    public class SeasonalModel
    {
        public int ProjectId { get; set; }
        public long ScenarioId { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfTimeIntervals { get; set; }
        public List<double> InitialElevation { get; set; }
        public List<bool> WaterDemandsUnits { get; set; }
        public List<List<double>> WaterDemands { get; set; }
        public List<double> CumulativeVolumeDiversionLicenses { get; set; }
        public List<double?> MaximalRateDiversionLicenses { get; set; }
        public List<double> ApportionmentAgreements { get; set; }
    }
}
