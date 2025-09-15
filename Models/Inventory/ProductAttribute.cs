using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Inventory
{
    public class ProductAttribute
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Attribute Name Is Requierd")]
        [StringLength(100, ErrorMessage = "Attribute Name Must Be Less Than 100 Characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Attribute Type Is Requierd")]
        [StringLength(50, ErrorMessage = "Attribute Type Must Be Less Than 50 Characters")]
        public string Type { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;


        public ICollection<VariantAttributeValue> ProductVariants { get; set; } = new List<VariantAttributeValue>();

    }
}
