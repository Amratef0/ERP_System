using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERP_System_Project.Models.Inventory;

namespace ERP_System_Project.Models
{
    public class PurchaseOrderItem
    {
        [Key]
        public int PoLineId { get; set; }

        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }

        public int LineNumber { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; }


        [StringLength(1000)] public string? Description { get; set; }

        [Column(TypeName = "decimal(15,4)")] public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(15,4)")] public decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(15,4)")] public decimal LineAmount { get; set; }
        [Column(TypeName = "decimal(15,4)")] public decimal ReceivedQuantity { get; set; } = 0;

        public int StatusId { get; set; }
        public PurchaseOrderItemStatusCode Status { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        public int? RequisitionId { get; set; }
        public InventoryRequisition? InventoryRequisition { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
