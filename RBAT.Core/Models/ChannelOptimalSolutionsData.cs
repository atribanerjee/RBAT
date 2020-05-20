using System;
using RBAT.Core.Clasess;

namespace RBAT.Core.Models
{
    public class ChannelOptimalSolutionsData : BaseEntityLongID
    {
        public long ChannelOptimalSolutionsID { get; set; }
        public ChannelOptimalSolutions ChannelOptimalSolutions { get; set; }
        public DateTime SurveyDate { get; set; }
        public double? OptimalValue { get; set; }
        public double? IdealValue { get; set; }
        public double? OptimalPowerValue { get; set; }
        public double? IdealPowerValue { get; set; }
    }
}
