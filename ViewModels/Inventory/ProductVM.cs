using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.ViewModels.Inventory
{
    public class ProductVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name Is Required")]
        [StringLength(100, ErrorMessage = "Category Name Must Be Less Than 100 Characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Product Description Is Required")]
        public string Description { get; set; } = null!;

        [DecimalPrecisionScale(10, 2)]
        [DisplayName("Unit Cost")]
        [Required(ErrorMessage = "Please Enter Unit Cost.")]

        public decimal UnitCost { get; set; } = 0;

        [DecimalPrecisionScale(10, 2)]
        [DisplayName("Standard Price")]
        [Required(ErrorMessage = "Please Enter a Standard Price.")]
        public decimal StandardPrice { get; set; }

        [Required(ErrorMessage = "Please select a brand.")]
        [DisplayName("Brand")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Please select a Category.")]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please Add Number Of Product Variants")]
        [DisplayName("Number Of Variants")]
        [Range(1, 3, ErrorMessage = "You can only add between 1 and 3 variants.")]
        public int NumberOfVariants { get; set; }

        public List<ProductVariantVM> ProductVariants { get; set; } = new List<ProductVariantVM>();


    }
}
