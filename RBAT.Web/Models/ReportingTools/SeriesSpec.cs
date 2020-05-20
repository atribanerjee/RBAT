using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Models.ReportingTools
{
    public class SeriesSpec
    {
        public ComponentType Type { get; set; }
        public long ComponentId { get; set; }
        public SolutionValueType ValueType { get; set; }
    }
}
