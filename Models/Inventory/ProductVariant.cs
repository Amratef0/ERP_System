using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.ValidationAttributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Inventory
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "SKU Is Requierd")]
        [StringLength(100, ErrorMessage = "SKU Must Be Less Than 100 Characters")]
        public string SKU { get; set; } = null!;

        [DecimalPrecisionScale(15, 4)]
        public decimal AdditionalPrice { get; set; } = 0;

        [DecimalPrecisionScale(15, 4)]
        public decimal Quantity { get; set; } = 0;

        [DecimalPrecisionScale(10, 4)]
        public decimal ReorderPoint { get; set; } = 0;
        public bool LowStockAlert { get; set; } = false;

        [DecimalPrecisionScale(10, 4)]
        public decimal MinStockLevel { get; set; }

        [DecimalPrecisionScale(10, 4)]
        public decimal MaxStockLevel { get; set; }
        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }


        public ICollection<VariantAttributeValue> ProductAttributes { get; set; } = new List<VariantAttributeValue>();
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
        public ICollection<InventoryRequisitionItem> InventoryRequestedVariantProducts { get; set; } = new List<InventoryRequisitionItem>();
        public ICollection<OrderItem> OrderedItems { get; set; } = new List<OrderItem>();



    }
}
