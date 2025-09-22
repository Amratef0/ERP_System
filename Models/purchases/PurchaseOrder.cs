using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int PurchaseOrderId { get; set; }

        [Required, StringLength(50)]
        public string PoNumber { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public int StatusId { get; set; }
        public PurchaseOrderStatusCode Status { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }

        

        [Column(TypeName = "decimal(15,6)")] public decimal ExchangeRate { get; set; } = 1;
        [Column(TypeName = "decimal(15,4)")] public decimal Subtotal { get; set; } = 0;
        [Column(TypeName = "decimal(15,4)")] public decimal TaxAmount { get; set; } = 0;
        [Column(TypeName = "decimal(15,4)")] public decimal TotalAmount { get; set; } = 0;
        [Column(TypeName = "decimal(15,4)")] public decimal ShippingCost { get; set; } = 0;

        public int PaymentTermsId { get; set; }
        public PaymentTerm PaymentTerms { get; set; }

        [StringLength(100)] public string? ShippingMethod { get; set; }
        [StringLength(1000)] public string? Notes { get; set; }

        public int CreatedBy { get; set; }
        public Employee CreatedByEmployee { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    }
}
