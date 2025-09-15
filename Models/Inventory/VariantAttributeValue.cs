using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Inventory
{
    public class VariantAttributeValue
    {
        [ForeignKey("ProductVariant")]
        public int Variant_Id { get; set; }
        public ProductVariant ProductVariant { get; set; }

        [ForeignKey("ProductAttribute")]
        public int Atrribute_Id { get; set; }
        public ProductAttribute ProductAttribute { get; set; }

        [Required(ErrorMessage = "Attribute Value Is Requierd")]
        [StringLength(255, ErrorMessage = "Attribute Value Must Be Less Than 255 Characters")]
        public string Attribute_Value { get; set; } = null!;
    }
}
