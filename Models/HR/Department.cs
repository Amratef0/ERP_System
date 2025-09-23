using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class Department
    {
        [Key]
        [Display(Name = "Department ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [MaxLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        [MinLength(2, ErrorMessage = "Department name must be at least 2 characters long.")]
        [Display(Name = "Department Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Department code is required.")]
        [MaxLength(50, ErrorMessage = "Department code cannot exceed 50 characters.")]
        [MinLength(1, ErrorMessage = "Department code must be at least 1 character long.")]
        [Display(Name = "Department Code")]
        public string Code { get; set; }

        [MaxLength(50, ErrorMessage = "Cost center code cannot exceed 50 characters.")]
        [MinLength(1, ErrorMessage = "Cost center code must be at least 1 character long.")]
        [Display(Name = "Cost Center Code")]
        public string? CostCenterCode { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("ParentDepartment")]
        [Display(Name = "Parent Department")]
        public int? ParentDepartmentId { get; set; }
        public Department? ParentDepartment { get; set; }

        [Display(Name = "Employees")]
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
