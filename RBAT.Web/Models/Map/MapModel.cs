using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Models.Map
{
    public class MapModel
    {
        public int? ProjectId { get; set; }
        public string ProjectMapData { get; set; }
        public ProjectNodeModelList ProjectNodes { get; set; }
        public ProjectChannelModelList ProjectChannels { get; set; }
    }
}
