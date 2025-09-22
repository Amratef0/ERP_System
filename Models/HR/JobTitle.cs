using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class JobTitle : IValidatableObject
    {
        [Key]
        [Display(Name = "Job Title ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Job title name is required.")]
        [MaxLength(100, ErrorMessage = "Job title name cannot exceed 100 characters.")]
        [MinLength(2, ErrorMessage = "Job title name must be at least 2 characters long.")]
        [Display(Name = "Job Title Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Job title code is required.")]
        [MaxLength(20, ErrorMessage = "Job title code cannot exceed 20 characters.")]
        [MinLength(1, ErrorMessage = "Job title code must be at least 1 character long.")]
        [Display(Name = "Job Title Code")]
        public string Code { get; set; }

        [Display(Name = "Job Grade")]
        public int? JobGrade { get; set; }

        [DecimalPrecisionScale(15, 4)]
        [Display(Name = "Minimum Salary")]
        public decimal? MinSalary { get; set; }

        [DecimalPrecisionScale(15, 4)]
        [Display(Name = "Maximum Salary")]
        public decimal? MaxSalary { get; set; }

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        // Navigation Properties
        [Display(Name = "Employees")]
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MaxSalary.HasValue && MinSalary.HasValue && MaxSalary < MinSalary)
            {
                yield return new ValidationResult(
                    "Maximum salary cannot be less than minimum salary.",
                    new[] { nameof(MaxSalary), nameof(MinSalary) }
                );
            }
        }
    }
}
