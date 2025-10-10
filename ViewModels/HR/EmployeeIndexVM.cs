using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.HR
{
    public class EmployeeIndexVM
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Branch")]
        public Branch Branch { get; set; }

        [Display(Name = "Job Title")]
        public JobTitle JobTitle { get; set; }

        [Display(Name = "Employee Type")]
        public EmployeeType Type { get; set; }

        [Display(Name = "Department")]
        public Department Department { get; set; }
    }
}
