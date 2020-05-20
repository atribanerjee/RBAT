using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RBAT.Core.Models
{
    public class NodeType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public const int NodeTypeReservoir = 1;
        public const int NodeTypeConsumptiveUse = 2;
        public const int NodeTypeJunction = 3;
    }
}
