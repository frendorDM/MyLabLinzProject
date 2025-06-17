using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabProject.Models.Customers
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = "Not set";

        [Required]
        [MaxLength(255)]
        public string Surname { get; set; } = "Not set";

        [MaxLength(255)]
        public string Lastname { get; set; } = "Not set";

        public DateTime DateOfBirthday { get; set; } 

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = "Not set";

        [MaxLength(255)]
        public string CountryOfResidence { get; set; } = "Not set";
    }
}
