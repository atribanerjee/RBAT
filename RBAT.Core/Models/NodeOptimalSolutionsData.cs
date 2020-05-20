using System;
using RBAT.Core.Clasess;

namespace RBAT.Core.Models
{
    public class NodeOptimalSolutionsData : BaseEntityLongID
    {
        public long NodeOptimalSolutionsID { get; set; }
        public NodeOptimalSolutions NodeOptimalSolutions { get; set; }
        public DateTime SurveyDate { get; set; }
        public double? OptimalValue { get; set; }
        public double? IdealValue { get; set; }
    }
}
