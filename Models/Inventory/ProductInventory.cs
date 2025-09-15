using ERP_System_Project.Models.ValidationAttributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Inventory
{
    public class ProductInventory
    {
        [Key]
        public int Id { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal QuantityOnHand { get; set; } = 0;

        
        [DecimalPrecisionScale(15, 4)]
        public decimal QuantityCommitted { get; set; } = 0;

        [DecimalPrecisionScale(15, 4)]
        public decimal QuantityAvailable { get; set; } = 0;

        [DecimalPrecisionScale(15, 4)]
        public decimal QuantityOnOrder { get; set; } = 0;
        public DateTime? LastCountDate { get; set; }
        public DateTime? LastReceiptDate { get; set; }
        public DateTime? LastIssueDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }


        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
