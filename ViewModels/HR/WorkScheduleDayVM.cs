using ERP_System_Project.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.ViewModels.HR
{
    public class WorkScheduleDayVM
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Day of the Week")]
        public Days Day { get; set; }

        [Display(Name = "Work Day?")]
        public bool IsWorkDay { get; set; }

        [Display(Name = "Work Start Time")]
        public TimeOnly? WorkStartTime { get; set; }

        [Display(Name = "Work End Time")]
        public TimeOnly? WorkEndTime { get; set; }

        // Navigation Properties
        [Required]
        public int WorkScheduleId { get; set; }
    }
}
