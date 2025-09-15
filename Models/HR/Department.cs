using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string Code { get; set; }

        [MaxLength(50)]
        [MinLength(1)]
        public string CostCenterCode { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey("ParentDepartment")]
        public int? ParentDepartmentId { get; set; }
        public Department ParentDepartment { get; set; }

        // Rest of navigation properties can be added here as needed
    }
}
