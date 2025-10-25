using ERP_System_Project.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.HR
{
    public class CreatePayrollRunVM
    {
        [Required(ErrorMessage = "Year is required.")]
        [Range(2000, 2100, ErrorMessage = "Year must be between 2000 and 2100.")]
        [Display(Name = "Year")]
        public int Year { get; set; } = DateTime.Now.Year;

        [Required(ErrorMessage = "Month is required.")]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        [Display(Name = "Month")]
        public int Month { get; set; } = DateTime.Now.Month;

        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM");
    }
}
