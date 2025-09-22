using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ERP_System_Project.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required, StringLength(50)]
        public string SupplierCode { get; set; }

        [Required, StringLength(255)]
        public string SupplierName { get; set; }

        public int SupplierCategoryId { get; set; }
        public SupplierCategory SupplierCategory { get; set; }

      
        [StringLength(100)] public string? PrimaryContactName { get; set; }
        [StringLength(100)] public string? PrimaryContactEmail { get; set; }
        [StringLength(50)] public string? PrimaryContactPhone { get; set; }

        [StringLength(100)] public string? SecondaryContactName { get; set; }
        [StringLength(100)] public string? SecondaryContactEmail { get; set; }
        [StringLength(50)] public string? SecondaryContactPhone { get; set; }

        [StringLength(100)] public string? TaxId { get; set; }

        [Column(TypeName = "decimal(15,4)")] public decimal CreditLimit { get; set; } = 0;
        [Column(TypeName = "decimal(15,4)")] public decimal CurrentBalance { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public ICollection<SupplierProduct> SupplierProducts { get; set; }
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
