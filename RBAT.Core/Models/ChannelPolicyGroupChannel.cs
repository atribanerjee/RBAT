using RBAT.Core.Clasess;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Models
{
    public class ChannelPolicyGroupChannel :  BaseEntityLongID
    {
        [Required]
        public long ChannelPolicyGroupID { get; set; }
        public ChannelPolicyGroup ChannelPolicyGroup { get; set; }

        [Required]
        public int ChannelID { get; set; }
        public Channel Channel { get; set; }

        [Required]
        public int Priority { get; set; }
    }
}
