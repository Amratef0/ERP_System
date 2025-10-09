using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_System_Project.Models.Inventory;

namespace ERP_System_Project.Models
{
    public class PurchaseOrder
    {
        [Key]
        public int PurchaseOrderId { get; set; }

        [Required, StringLength(50)]
        public string? PoNumber { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        // Warehouse
        [Required]
        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }

        // Supplier
        [Required]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        // Product in Warehouse
        [Required]
        public int WarehouseProductId { get; set; }
        public WarehouseProduct? WarehouseProduct { get; set; }

        [Required]
        public decimal Quantity { get; set; }  // كمية الطلب

        public int StatusId { get; set; }
        public int PaymentTermsId { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
