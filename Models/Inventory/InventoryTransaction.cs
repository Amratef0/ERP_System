using ERP_System_Project.Models.HR;
using ERP_System_Project.Models.ValidationAttributes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Inventory
{
    public class InventoryTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Transaction Date is required")]
        public DateTime TransactionDate { get; set; }

        [DecimalPrecisionScale(19,4)]
        [Required(ErrorMessage = "Quantity is required")]
        public decimal Quantity { get; set; }

        [DecimalPrecisionScale(19, 4)]
        public decimal? UnitCost { get; set; }

        [DecimalPrecisionScale(19, 4)]
        public decimal? TotalCost { get; set; }

        [StringLength(50, ErrorMessage = "Reference Type Must Be Less Than 50 Characters")]
        public string? ReferenceType { get; set; }
        public int? ReferenceId { get; set; }

        [StringLength(1000, ErrorMessage = "Reference Type Must Be Less Than 1000 Characters")]
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Variant")]
        public int VariantId { get; set; }
        public ProductVariant Variant { get; set; }

        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        [ForeignKey("TransactionType")]
        public int TransactionTypeId { get; set; }
        public InventoryTransactionType TransactionType { get; set; }

        [ForeignKey("Employee")]
        public int CreatedBy { get; set; }
        public Employee Employee { get; set; }

    }
}
