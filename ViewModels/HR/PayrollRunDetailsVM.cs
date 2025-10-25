using ERP_System_Project.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.HR
{
    public class PayrollRunDetailsVM
    {
        public int Id { get; set; }

        [Display(Name = "Payroll Run Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Period Start")]
        public DateOnly PeriodStart { get; set; }

        [Display(Name = "Period End")]
        public DateOnly PeriodEnd { get; set; }

        [Display(Name = "Processed Date")]
        public DateTime? ProcessedDate { get; set; }

        [Display(Name = "Is Locked")]
        public bool IsLocked { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Status")]
        public string Status => IsLocked ? "Confirmed" : "Draft";

        public int Year => PeriodStart.Year;
        public int Month => PeriodStart.Month;

        // Entries
        public List<PayrollEntryVM> Entries { get; set; } = new();

        // Currency breakdown
        [Display(Name = "Currency Totals")]
        public Dictionary<string, decimal> CurrencyTotals { get; set; } = new();

        // List of all currencies used in this run
        public List<Currency> CurrenciesUsed { get; set; } = new();
    }
}
