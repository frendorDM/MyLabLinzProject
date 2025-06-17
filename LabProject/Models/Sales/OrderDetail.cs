using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabProject.Models.Sales
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required]
        public int Sku { get; set; }

        [Required]
        [MaxLength(255)]
        public string SkuName { get; set; } = "Not set";

        [Required]
        public int Amount { get; set; }

        [Required]
        public double UnitPrice { get; set; }

        public virtual Order? Order { get; set; }

    }
}