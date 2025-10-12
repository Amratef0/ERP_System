using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class WorkScheduleDay
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Day of the Week")]
        public DayOfWeek Day { get; set; }

        [Display(Name = "Work Day?")]
        public bool IsWorkDay { get; set; }

        [Display(Name = "Work Start Time")]
        public TimeOnly? WorkStartTime { get; set; }

        [Display(Name = "Work End Time")]
        public TimeOnly? WorkEndTime { get; set; }

        // Navigation Properties
        [ForeignKey("WorkSchedule")]
        public int WorkScheduleId { get; set; }
        public virtual WorkSchedule WorkSchedule { get; set; }
    }
}
