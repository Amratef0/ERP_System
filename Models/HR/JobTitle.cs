using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class JobTitle : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(1)]
        public string Code { get; set; }

        public int? JobGrade { get; set; }

        [DecimalPrecisionScale(15, 4)] // Custom validation attribute to enforce precision and scale
        public decimal? MinSalary { get; set; }

        [DecimalPrecisionScale(15, 4)] // Custom validation attribute to enforce precision and scale
        public decimal? MaxSalary { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public bool IsActive { get; set; }

        // Rest of navigation properties can be added here as needed

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MaxSalary.HasValue && MinSalary.HasValue && MaxSalary < MinSalary)
            {
                yield return new ValidationResult(
                    "Maximum Salary cannot be less than Minimum Salary.",
                    new[] { nameof(MaxSalary), nameof(MinSalary) }
                );
            }
        }
    }
}
