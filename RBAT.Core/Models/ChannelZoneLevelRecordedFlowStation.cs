using RBAT.Core.Clasess;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Models
{
    public class ChannelZoneLevelRecordedFlowStation : BaseEntityLongID
    {
        [Required]
        public long ChannelPolicyGroupId { get; set; }
        public ChannelPolicyGroup ChannelPolicyGroup { get; set; }

        [Required]
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }

        public int? Zone1Id { get; set; }
        public int? Zone2Id { get; set; }
        public int? Zone3Id { get; set; }

        public int? RecordedFlowStation1Id { get; set; }
        public int? RecordedFlowStation2Id { get; set; }
        public int? RecordedFlowStation3Id { get; set; }
    }
}

