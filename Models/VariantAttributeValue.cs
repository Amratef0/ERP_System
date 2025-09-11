using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models
{
    public class VariantAttributeValue
    {
        [ForeignKey("ProductVariant")]
        public int Variant_Id { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!;

        [ForeignKey("ProductAttribute")]
        public int Atrribute_Id { get; set; }
        public ProductAttribute ProductAttribute { get; set; } = null!;

        [Required(ErrorMessage = "Attribute Value Is Requierd")]
        [StringLength(255, ErrorMessage = "Attribute Value Must Be Less Than 255 Characters")]
        public string Attribute_Value { get; set; } = null!;
    }


    /*
     variant_id INT NOT NULL,
    attribute_id INT NOT NULL,
    attribute_value NVARCHAR(255) NOT NULL,
    CONSTRAINT pk_variant_attribute PRIMARY KEY (variant_id, attribute_id),
    CONSTRAINT fk_vav_variant FOREIGN KEY (variant_id) REFERENCES product_variants(variant_id),
    CONSTRAINT fk_vav_attribute FOREIGN KEY (attribute_id) REFERENCES product_attributes(attribute_id)
     */
}
