using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Inventory
{
    public class InventoryTransaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [ForeignKey("WarehouseProduct")]
        public int WarehouseProductId { get; set; }
        public WarehouseProduct WarehouseProduct { get; set; } = null!;

        public decimal Quantity { get; set; }

        public string TransactionType { get; set; } = "Purchase"; // ممكن يكون "Sale" كمان مستقبلاً

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public string? Notes { get; set; }
    }
}
