using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models
{
    public class PaymentTerm
    {
        [Key]
        public int PaymentTermsId { get; set; }

        [Required, StringLength(50)]
        public string TermsCode { get; set; }

        [Required, StringLength(255)]
        public string TermsDescription { get; set; }

        public int? DueDays { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
