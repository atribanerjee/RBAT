using System.ComponentModel;

namespace RBAT.Web.Extensions
{
    public enum ResultValueType
    {
        [Description("Ideal")]
        Ideal,
        [Description("Optimal")]
        Optimal,
        [Description("Ideal Power")]
        IdealPower,
        [Description("Optimal Power")]
        OptimalPower
    }
}
