using ERP_System_Project.Models.ValidationAttributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Inventory
{
    public class Product
    {
        [Key]
        public int Product_Id { get; set; }

        [Required(ErrorMessage = "Product Name Is Required")]
        [StringLength(100, ErrorMessage = "Category Name Must Be Less Than 100 Characters")]
        public string Product_Name { get; set; } = null!;

        [Required(ErrorMessage = "Product Description Is Required")]
        public string Product_Description { get; set;} = null!;

        [Range(0.01, 10_000_000, ErrorMessage = "The Price must be greater than 0.")]
        [Precision(10, 2)]
        public decimal Unit_Cost { get; set; } = 0;

        [DecimalPrecisionScale(10,2)]
        public decimal Standard_Price { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? heigth { get; set; }
        public bool Is_Active { get; set; } = true;
        public decimal Reorder_Point { get; set; } = 0;
        public bool Low_Stock_Alert { get; set; } = false;
        public DateTime Created_Date { get; set; } = DateTime.Now;
        public DateTime? Modified_Date { get; set; }

        [ForeignKey("Brand")]
        public int Brand_Id { get; set; }
        public Brand Brand { get; set; }
        
        [ForeignKey("Category")]
        public int Category_Id { get; set; }
        public Category Category { get; set; }

        [ForeignKey("Product_Type")]
        public int? Product_Type_Id { get; set; }
        public ProductType? Product_Type { get; set; }

        [ForeignKey("Unit_Of_Measure")]
        public int? UOM_Id { get; set; }
        public UnitOfMeasure? Unit_Of_Measure { get; set; }


        public ICollection<ProductVariant> productVariants { get; set; } = new List<ProductVariant>();
        public ICollection<ProductInventory> Warehouses { get; set; } = new List<ProductInventory>();
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
    }
}
