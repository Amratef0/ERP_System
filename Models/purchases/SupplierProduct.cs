using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models
{
    public class SupplierProduct
    {
        [Key]
        public int SupplierProductId { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
 
        [StringLength(100)]
        public string? SupplierProductCode { get; set; }

        [Column(TypeName = "decimal(15,4)")] public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(15,4)")] public decimal MinimumOrderQuantity { get; set; } = 0;

        public int? LeadTimeDays { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
