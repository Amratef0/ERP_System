using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models
{
    public class Brand
    {
        [Key]
        public int Brand_Id { get; set; }

        [Required(ErrorMessage = "Brand Name Is Requierd")]
        [StringLength(255, ErrorMessage = "Brand Name Must Be Less Than 255 Characters")]
        public string Brand_Name { get; set; } = null!;

        [Required(ErrorMessage = "Brand Description Is Requierd")]
        public string Brand_Description { get; set; } = null!;

        [Required(ErrorMessage = "Logo URL Is Requierd")]
        [StringLength(255, ErrorMessage = "Logo URL Must Be Less Than 255 Characters")]
        public string Logo_URL { get; set; } = null!;

        [StringLength(255, ErrorMessage = "Website URL Must Be Less Than 255 Characters")]
        public string Website_URL { get; set; }
        public bool Is_Active { get; set; } = true;
        public DateTime Created_Date { get; set; } = DateTime.Now;


        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
