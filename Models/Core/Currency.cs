using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.HR;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Core
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Currency Code Is Required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency Code Must be 3 characters")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "Currency Name Is Required")]
        [StringLength(30, ErrorMessage = "Currency Name must be less than 30 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Symbol Is Required")]
        [StringLength(5, ErrorMessage = "Symbol must be less than 5 characters")]
        public string Symbol { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }


        // Navigation Properties
        [Display(Name = "Orders with this Currency")]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        [Display(Name = "Payroll Runs with this Currency")]
        public virtual ICollection<PayrollRun> PayrollRuns { get; set; } = new List<PayrollRun>();

        [Display(Name = "Payroll Entries with this Currency")]
        public virtual ICollection<PayrollEntry> PayrollEntries { get; set; } = new List<PayrollEntry>();

        [Display(Name = "Employees with this Currency as Salary Currency")]
        public virtual ICollection<Employee> EmployeesByCurrency { get; set; } = new List<Employee>();
    }
}
