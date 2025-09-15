using ERP_System_Project.Models.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models
{
    public class Address
    {
        [Key]
        public int Address_Id { get; set; }

        [Required(ErrorMessage = "Address Is Required")]
        [StringLength(255, ErrorMessage = "Address must be less than 255 characters")]
        public string Address_Line1 { get; set; } = null!;

        [StringLength(255, ErrorMessage = "Second Address must be less than 255 characters")]
        public string? Address_Line2 { get; set; }

        [Required(ErrorMessage = "City Name Is Required")]
        [StringLength(30, ErrorMessage = "City must be less than 30 characters")]
        public string City { get; set; } = null!;

        [StringLength(30, ErrorMessage = "State Province must be less than 30 characters")]
        public string? State_Province { get; set; }

        [StringLength(20, ErrorMessage = "Postal Code must be less than 20 characters")]
        public string? Postal_Code { get; set; }

        [StringLength(20, ErrorMessage = "Address Type Name must be less than 20 characters")]
        public string? Address_Type { get; set; }
        public bool Is_Active { get; set; } = true;

        [ForeignKey("Country")]
        public int Country_Id { get; set; }
        public Country Country { get; set; }

        public ICollection<Branch> branches { get; set; } = new List<Branch>();
        public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
    }
}
