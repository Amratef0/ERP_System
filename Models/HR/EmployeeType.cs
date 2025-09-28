using ERP_System_Project.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.HR
{
    public class EmployeeType : ISoftDeletable
    {
        [Key]
        [Display(Name = "Employee Type ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee type name is required.")]
        [MaxLength(50, ErrorMessage = "Employee type name cannot exceed 50 characters.")]
        [MinLength(2, ErrorMessage = "Employee type name must be at least 2 characters long.")]
        [Display(Name = "Employee Type Name")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        // Navigation Properties
        [Display(Name = "Employees")]
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
