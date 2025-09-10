using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models
{
    public class Brand
    {
        [Key]
        public int brand_Id { get; set; }

        [Required(ErrorMessage = "Brand Name Is Requierd")]
        [StringLength(255, ErrorMessage = "Brand Name Must Be Less Than 255 Characters")]
        public string brand_name { get; set; } = null!;

        [Required(ErrorMessage = "Brand Description Is Requierd")]
        public string brand_description { get; set; } = null!;

        [Required(ErrorMessage = "Logo URL Is Requierd")]
        [StringLength(255, ErrorMessage = "Logo URL Must Be Less Than 255 Characters")]
        public string logo_url { get; set; } = null!;

        [StringLength(255, ErrorMessage = "Website URL Must Be Less Than 255 Characters")]
        public string website_url { get; set; }
        public bool is_active { get; set; } = true;
        public DateTime created_date { get; set; } = DateTime.Now;
    }
}
