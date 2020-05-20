using System.Collections.Generic;
using System.Linq;

namespace RBAT.Web.Models.ReportingTools
{
    public class ProjectScenarioModel
    {
        //public long Id { get; set; }

        public string Name { get; set; }
    }

    public class ProjectScenarioModelList : List<ProjectScenarioModel>
    {

    }

    internal static partial class ViewModelExtensions
    {
        internal static ProjectScenarioModel ToProjectScenarioModel(this string source)
        {
            if (source == null)
            {
                return null;
            }

            return new ProjectScenarioModel
            {
                Name = source
            };
        }

        internal static ProjectScenarioModelList ToProjectScenarioViewModelList(this IList<string> source)
        {
            var rv = new ProjectScenarioModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToProjectScenarioModel());
                }
            }

            return rv;
        }
    }
}
