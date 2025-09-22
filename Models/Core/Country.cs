using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Core
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Country Code Is Required")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Country Code Must be 2 characters")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "Country Name Is Required")]
        [StringLength(30, ErrorMessage = "Country Name Must be less than 30 characters")]
        public string Name { get; set; } = null!;

        [StringLength(10, ErrorMessage = "Phone Code Must be less than 10 characters")]
        public string? PhoneCode { get; set; }

        // Navigation Properties
        public ICollection<Address> Addresses { get; set; } = new List<Address>();

    }
}
