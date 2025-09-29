using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Inventory
{
    public class ProductType
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Type Name Is Required")]
        [StringLength(100, ErrorMessage = "Product Type Name Must Be Less Than 100 Characters")]
        public string Name { get; set; } = null!;

        [StringLength(255, ErrorMessage = "Product Type Description Must Be Less Than 255 Characters")]
        public string? Description { get; set;}
        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set;} = new List<Product>();
    }
}
