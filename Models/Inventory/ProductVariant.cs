using ERP_System_Project.Models.ValidationAttributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Inventory
{
    public class ProductVariant
    {
        [Key]
        public int Variant_Id { get; set; }

        [Required(ErrorMessage = "SKU Is Requierd")]
        [StringLength(100, ErrorMessage = "SKU Must Be Less Than 100 Characters")]
        public string SKU { get; set; } = null!;

        [DecimalPrecisionScale(15, 4)]
        public decimal Additional_Price { get; set; } = 0;

        [DecimalPrecisionScale(15, 4)]
        public decimal Quantity { get; set; } = 0;

        [DecimalPrecisionScale(10, 4)]
        public decimal Reorder_Point { get; set; } = 0;
        public bool Low_Stock_Alert { get; set; } = false;

        [DecimalPrecisionScale(10, 4)]
        public decimal Min_Stock_Level { get; set; }

        [DecimalPrecisionScale(10, 4)]
        public decimal Max_Stock_Level { get; set; }
        public bool Is_Default { get; set; } = false;
        public bool Is_Active { get; set; } = true;
        public DateTime Created_Date { get; set; } = DateTime.Now;

        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public Product Product { get; set; }


        public ICollection<VariantAttributeValue> ProductAttributes { get; set; } = new List<VariantAttributeValue>();
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    }
}
