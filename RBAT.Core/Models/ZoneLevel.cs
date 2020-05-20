using System.Collections.Generic;

namespace RBAT.Core.Models
{
    public class ZoneLevel
    {
        public List<double> ZoneLevelsAboveIdeal { get; set; }
        public List<double> IdealZone { get; set; }
        public List<double> ZoneLevelsBelowIdeal { get; set; }
    }

    public class ZoneLevelWithTimeComponent : ZoneLevel
    {
        public int Year { get; set; }
        public int TimeComponentValue { get; set; }        
    }
}
