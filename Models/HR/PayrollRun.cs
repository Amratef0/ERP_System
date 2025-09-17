using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class PayrollRun
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateOnly PeriodStart { get; set; }

        [Required]
        public DateOnly PeriodEnd { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public bool IsLocked { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal? TotalAmount { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation properties

        [Required]
        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}
