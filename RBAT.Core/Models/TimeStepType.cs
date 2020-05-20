using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RBAT.Core.Models {

    public class TimeStepType {

        public bool CalculationFlag { get; set; }

        public bool DataFlag { get; set; }

        [DatabaseGenerated( DatabaseGeneratedOption.None )]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength( 50 )]
        public string Name { get; set; }

    }

}
