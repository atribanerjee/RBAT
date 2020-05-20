using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RBAT.Web.Models.ReportingTools
{
    public enum ChartType
    {
        [Display(Name = "Time Series")]
        TimeSeries,
        Stepped,
        Exceedence
    }
}
