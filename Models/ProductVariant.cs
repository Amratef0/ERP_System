using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models
{
    public class ProductVariant
    {
        [Key]
        public int Variant_Id { get; set; }

        [Required(ErrorMessage = "SKU Is Requierd")]
        [StringLength(100, ErrorMessage = "SKU Must Be Less Than 100 Characters")]
        public string SKU { get; set; } = null!;

        [Range(0.01, 10_000_000_000, ErrorMessage = "The Additional Price must be greater than 0 and less than 11 digit before (,)")]
        [Precision(15, 4)]
        public decimal Additional_Price { get; set; } = 0;

        [Range(0.01, 10_000_000_000, ErrorMessage = "The Quantity must be greater than 0 and less than 11 digit before (,)")]
        [Precision(15, 4)]
        public decimal Quantity { get; set; } = 0;

        [Range(0.01, 1_000_000, ErrorMessage = "The Reorder Point must be greater than 0 and less than 6 digit before (,)")]
        [Precision(10, 4)]
        public decimal Reorder_Point { get; set; } = 0;
        public bool Low_Stock_Alert { get; set; } = false;

        [Range(0.01, 1_000_000, ErrorMessage = "The Min Stock Level must be greater than 0 and less than 6 digit before (,)")]
        [Precision(10, 4)]
        public decimal Min_Stock_Level { get; set; }

        [Range(0.01, 1_000_000, ErrorMessage = "The Max Stock Level must be greater than 0 and less than 6 digit before (,)")]
        [Precision(10, 4)]
        public decimal Max_Stock_Level { get; set; }
        public bool Is_Default { get; set; } = false;
        public bool Is_Active { get; set; } = true;
        public DateTime Created_Date { get; set; } = DateTime.Now;

        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public Product Product { get; set; } = null!;


        public ICollection<VariantAttributeValue> ProductAttributes { get; set; } = new List<VariantAttributeValue>();
    }
}
