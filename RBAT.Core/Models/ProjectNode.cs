using RBAT.Core.Clasess;

namespace RBAT.Core.Models
{
    public class ProjectNode : BaseEntityIntID
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int NodeId { get; set; }
        public Node Node { get; set; }     
    }
}
