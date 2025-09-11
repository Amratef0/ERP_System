using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models
{
    public class ProductAttribute
    {
        [Key]
        public int Atrribute_Id { get; set; }

        [Required(ErrorMessage = "Attribute Name Is Requierd")]
        [StringLength(100, ErrorMessage = "Attribute Name Must Be Less Than 100 Characters")]
        public string Attribute_Name { get; set; } = null!;

        [Required(ErrorMessage = "Attribute Type Is Requierd")]
        [StringLength(50, ErrorMessage = "Attribute Type Must Be Less Than 50 Characters")]
        public string Attribute_Type { get; set; } = null!;
        public bool Is_Active { get; set; } = true;
        public DateTime Created_Date { get; set; } = DateTime.Now;


        public ICollection<VariantAttributeValue> ProductVariants { get; set; } = new List<VariantAttributeValue>();

    }
}
