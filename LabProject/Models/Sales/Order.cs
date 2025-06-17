using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabProject.Models.Sales
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual ICollection<OrderDetail> OrderDetail { get; } = new List<OrderDetail>();
    }
}