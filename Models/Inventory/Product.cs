using ERP_System_Project.Models.ValidationAttributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Inventory
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name Is Required")]
        [StringLength(100, ErrorMessage = "Category Name Must Be Less Than 100 Characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Product Description Is Required")]
        public string Description { get; set;} = null!;

        [Range(0.01, 10_000_000, ErrorMessage = "The Price must be greater than 0.")]
        [Precision(10, 2)]
        public decimal UnitCost { get; set; } = 0;

        [DecimalPrecisionScale(10,2)]
        public decimal StandardPrice { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Heigth { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal ReorderPoint { get; set; } = 0;
        public bool LowStockAlert { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("ProductType")]
        public int? ProductTypeId { get; set; }
        public ProductType? ProductType { get; set; }

        [ForeignKey("UnitOfMeasure")]
        public int? UOMId { get; set; }
        public UnitOfMeasure? UnitOfMeasure { get; set; }


        public ICollection<ProductVariant> productVariants { get; set; } = new List<ProductVariant>();
        public ICollection<ProductInventory> Warehouses { get; set; } = new List<ProductInventory>();
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
    }
}
