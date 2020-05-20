using RBAT.Core.Clasess;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Models
{
    public class NodeZoneWeight : BaseEntityLongID
    {
        [Required]
        public long NodePolicyGroupID { get; set; }
        public NodePolicyGroup NodePolicyGroup { get; set; }

        public double? ZoneAboveIdeal6 { get; set; }
        public double? ZoneAboveIdeal5 { get; set; }
        public double? ZoneAboveIdeal4 { get; set; }
        public double? ZoneAboveIdeal3 { get; set; }
        public double? ZoneAboveIdeal2 { get; set; }
        public double? ZoneAboveIdeal1 { get; set; }
        public double? ZoneBelowIdeal1 { get; set; }
        public double? ZoneBelowIdeal2 { get; set; }
        public double? ZoneBelowIdeal3 { get; set; }
        public double? ZoneBelowIdeal4 { get; set; }
        public double? ZoneBelowIdeal5 { get; set; }
        public double? ZoneBelowIdeal6 { get; set; }
        public double? ZoneBelowIdeal7 { get; set; }
        public double? ZoneBelowIdeal8 { get; set; }
        public double? ZoneBelowIdeal9 { get; set; }
        public double? ZoneBelowIdeal10 { get; set; }
    }
}
