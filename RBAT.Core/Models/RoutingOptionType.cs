using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RBAT.Core.Models {

    public class RoutingOptionType {

        [Required]
        [StringLength( 250 )]
        public string Description { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength( 50 )]
        public string Name { get; set; }

    }

}
