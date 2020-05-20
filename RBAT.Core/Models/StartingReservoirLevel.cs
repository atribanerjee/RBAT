using RBAT.Core.Clasess;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Models
{
    public class StartingReservoirLevel : BaseEntityLongID
    {
        [Required]
        public long ScenarioId { get; set; }
        public Scenario Scenario { get; set; }

        [Required]
        public int NodeId { get; set; }
        public Node Node { get; set; }

        [Required]
        public double InitialElevation {get; set;}
    }
}
