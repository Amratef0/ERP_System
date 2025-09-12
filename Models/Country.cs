using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models
{
    public class Country
    {
        [Key]
        public int Country_Id { get; set; }

        [Required(ErrorMessage = "Country Code Is Required")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Country Code Must be 2 characters")]
        public string Country_Code { get; set; } = null!;

        [Required(ErrorMessage = "Country Name Is Required")]
        [StringLength(30, ErrorMessage = "Country Name Must be less than 30 characters")]
        public string Country_Name { get; set; } = null!;

        [StringLength(10, ErrorMessage = "Phone Code Must be less than 10 characters")]
        public string? Phone_Code { get; set; }


        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        
    }
}
