using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.HR
{
    public class WorkScheduleDay : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a day of the week.")]
        [Display(Name = "Day of the Week")]
        public Days Day { get; set; }

        [Required(ErrorMessage = "Please specify if this is a work day.")]
        [Display(Name = "Work Day?")]
        public bool IsWorkDay { get; set; }

        [Display(Name = "Work Start Time")]
        public TimeOnly? WorkStartTime { get; set; }

        [Display(Name = "Work End Time")]
        public TimeOnly? WorkEndTime { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Rule 1: If it's a workday, start and end times are required.
            if (IsWorkDay && (!WorkStartTime.HasValue || !WorkEndTime.HasValue))
            {
                yield return new ValidationResult("Start and End times are required for a workday.",
                    new[] { nameof(WorkStartTime), nameof(WorkEndTime) });
            }

            // Rule 2: If it's NOT a workday, start and end times should be null.
            if (!IsWorkDay && (WorkStartTime.HasValue || WorkEndTime.HasValue))
            {
                yield return new ValidationResult("Start and End times should not be set for a day off.",
                    new[] { nameof(WorkStartTime), nameof(WorkEndTime) });
            }

            // Rule 3: Ensure end time is after start time.
            if (WorkStartTime.HasValue && WorkEndTime.HasValue && WorkEndTime.Value <= WorkStartTime.Value)
            {
                yield return new ValidationResult("End time must be after start time.",
                    new[] { nameof(WorkEndTime) });
            }
        }
    }
}
