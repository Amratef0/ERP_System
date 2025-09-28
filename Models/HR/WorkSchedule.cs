using ERP_System_Project.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.HR
{
    public class WorkSchedule : ISoftDeletable
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name for the schedule.")]
        [StringLength(100)]
        [Display(Name = "Schedule Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        // Navigation Property
        public virtual ICollection<WorkScheduleDay> ScheduleDays { get; set; } = new List<WorkScheduleDay>();
    }
}
