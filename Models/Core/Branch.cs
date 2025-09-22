using ERP_System_Project.Models.HR;
using ERP_System_Project.Models.Inventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Core
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Branch Name Is Required")]
        [StringLength(100, ErrorMessage = "Branch Name Must be less than 100 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Branch Code Is Required")]
        [StringLength(10, ErrorMessage = "Branch Code Must be less than 10 characters")]
        public string Code { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Phone Number Must be less than 50 Number")]
        [Phone(ErrorMessage = "Invalid Phone Format")]
        public string? Number { get; set; }

        [StringLength(50, ErrorMessage = "Email Must be less than 30 characters")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string? Email { get; set; }
        public bool IsMainBranch { get; set; } = false;
        public bool IsActive { get; set; } = true;


        // Navigation Properties 
        [ForeignKey("Address")]
        [Display(Name = "Address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

        [Display(Name = "Warehouses")]
        public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();

        [Display(Name = "Employees")]
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
