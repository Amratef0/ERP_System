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
        public int Transaction_Id { get; set; }

        [Required(ErrorMessage = "Transaction Date is required")]
        public DateTime Transaction_Date { get; set; }

        [DecimalPrecisionScale(19,4)]
        [Required(ErrorMessage = "Quantity is required")]
        public decimal Quantity { get; set; }

        [DecimalPrecisionScale(19, 4)]
        public decimal? Unit_Cost { get; set; }

        [DecimalPrecisionScale(19, 4)]
        public decimal? Total_Cost { get; set; }

        [StringLength(50, ErrorMessage = "Reference Type Must Be Less Than 50 Characters")]
        public string? Reference_Type { get; set; }
        public int? Reference_Id { get; set; }

        [StringLength(1000, ErrorMessage = "Reference Type Must Be Less Than 1000 Characters")]
        public string Comments { get; set; }
        public DateTime Created_Date { get; set; } = DateTime.Now;

        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Variant")]
        public int Variant_Id { get; set; }
        public ProductVariant Variant { get; set; }

        [ForeignKey("Warehouse")]
        public int Warehouse_Id { get; set; }
        public Warehouse Warehouse { get; set; }

        [ForeignKey("TransactionType")]
        public int Transaction_Type_Id { get; set; }
        public InventoryTransactionType TransactionType { get; set; }

        [ForeignKey("Employee")]
        public int Created_By { get; set; }
        public Employee Employee { get; set; }

    }
}
