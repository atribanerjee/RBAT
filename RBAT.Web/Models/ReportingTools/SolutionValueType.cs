using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RBAT.Web.Models.ReportingTools
{
    public enum SolutionValueType
    {
        Ideal,
        [Display(Name = "Simulated")]
        Optimal
    }
}
