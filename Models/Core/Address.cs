using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Models.Inventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Core
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Address Is Required")]
        [StringLength(255, ErrorMessage = "Address must be less than 255 characters")]
        public string Line1 { get; set; } = null!;

        [StringLength(255, ErrorMessage = "Second Address must be less than 255 characters")]
        public string? Line2 { get; set; }

        [Required(ErrorMessage = "City Name Is Required")]
        [StringLength(30, ErrorMessage = "City must be less than 30 characters")]
        public string City { get; set; } = null!;

        [StringLength(30, ErrorMessage = "State Province must be less than 30 characters")]
        public string? StateProvince { get; set; }

        [StringLength(20, ErrorMessage = "Postal Code must be less than 20 characters")]
        public string? PostalCode { get; set; }

        [StringLength(20, ErrorMessage = "Address Type Name must be less than 20 characters")]
        public string? AddressType { get; set; }
        public bool IsActive { get; set; } = true;



        // Navigation Properties
        [ForeignKey("Country")]
        [Required(ErrorMessage = "Country is required")]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        [Display(Name = "Branches with this Address")]
        public ICollection<Branch> Branches { get; set; } = new List<Branch>();

        [Display(Name = "Warehouses with this Address")]
        public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();

        [Display(Name = "Employees with this Address")]
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
