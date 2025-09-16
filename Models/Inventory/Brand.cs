using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Inventory
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Brand Name Is Required")]
        [StringLength(255, ErrorMessage = "Brand Name Must Be Less Than 255 Characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Brand Description Is Required")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Logo URL Is Required")]
        [StringLength(255, ErrorMessage = "Logo URL Must Be Less Than 255 Characters")]
        public string LogoURL { get; set; } = null!;

        [StringLength(255, ErrorMessage = "Website URL Must Be Less Than 255 Characters")]
        public string? WebsiteURL { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;


        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
