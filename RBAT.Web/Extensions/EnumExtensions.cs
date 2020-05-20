using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;


namespace RBAT.Web.Extensions
{
    public class EnumExtensions
    {
        // Helper method to display the name of the enum values.
        public static string GetDisplayName(Enum value)
        {
            return value.GetType()?
           .GetMember(value.ToString())?.First()?
           .GetCustomAttribute<DisplayAttribute>()?
           .Name;
        }
    }
}
