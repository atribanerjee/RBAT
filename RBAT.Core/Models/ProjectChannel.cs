using RBAT.Core.Clasess;
using System.Collections.Generic;

namespace RBAT.Core.Models
{
   public class ProjectChannel : BaseEntityIntID
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}
