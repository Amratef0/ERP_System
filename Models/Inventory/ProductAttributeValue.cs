using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Inventory
{
    public class ProductAttributeValue
    {
        [ForeignKey("ProductVariant")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("ProductAttribute")]
        public int AtrributeId { get; set; }
        public ProductAttribute ProductAttribute { get; set; }

        [Required(ErrorMessage = "Attribute Value Is Requierd")]
        [StringLength(255, ErrorMessage = "Attribute Value Must Be Less Than 255 Characters")]
        public string Value { get; set; } = null!;
    }
}
