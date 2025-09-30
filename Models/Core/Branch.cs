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

        [Display(Name = "Branch Name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Branch Code")]
        public string Code { get; set; } = null!;

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Is Main Branch?")]
        public bool IsMainBranch { get; set; } = false;

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }


        // Navigation Properties 
        [ForeignKey("Address")]
        [Display(Name = "Address")]
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        [Display(Name = "Warehouses")]
        public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();

        [Display(Name = "Employees")]
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
