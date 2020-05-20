using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Models.ReportingTools
{
    public class ComponentViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ComponentType Type { get; set; }

        public bool IsNetEvaporation { get; set; }

        public long NodeId { get; set; }

        public long ChannelId { get; set; }
    }

    public class ComponentViewModelList : List<ComponentViewModel>
    {

    }

    internal static partial class ViewModelExtensions
    {
        // Node Components
        internal static ComponentViewModel ToComponentViewModel(this RBAT.Core.Models.NodeOptimalSolutions source)
        {
            if (source == null)
            {
                return null;
            }

            return new ComponentViewModel
            {
                Id = source.Id,
                Name = source.IsNetEvaporation ? source.Node.Name + " (Net Evap)" : source.Node.Name,
                Type = ComponentType.Node,
                IsNetEvaporation = source.IsNetEvaporation,
                NodeId = source.NodeID
            };
        }

        internal static ComponentViewModelList ToComponentViewModelList(this IEnumerable<RBAT.Core.Models.NodeOptimalSolutions> source)
        {
            var rv = new ComponentViewModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToComponentViewModel());
                }
            }

            return rv;
        }

        // Channel Components
        internal static ComponentViewModel ToComponentViewModel(this RBAT.Core.Models.ChannelOptimalSolutions source)
        {
            if (source == null)
            {
                return null;
            }

            return new ComponentViewModel
            {
                Id = source.Id,
                Name = source.Channel.Name,
                Type = ComponentType.Channel,
                ChannelId = source.ChannelID
            };
        }

        internal static ComponentViewModelList ToComponentViewModelList(this IEnumerable<RBAT.Core.Models.ChannelOptimalSolutions> source)
        {
            var rv = new ComponentViewModelList();
            if (source != null)
            {
                foreach (var item in source)
                {
                    rv.Add(item.ToComponentViewModel());
                }
            }

            return rv;
        }

    }
}
