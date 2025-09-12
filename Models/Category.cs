using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models
{
    public class Category
    {
        [Key]
        public int Category_Id { get; set; }

        [Required(ErrorMessage = "Category Name Is Required")]
        [StringLength(50, ErrorMessage = "Category Name Must Be Less Than 50 Characters")]
        public string Category_Name { get; set; } = null!;

        [Required(ErrorMessage = "Category Description Is Required")]
        public string Category_Description { get; set;} = null!;
        public string Category_Image_URL { get; set; }
        public bool Is_Active { get; set; } = true;
        public DateTime Created_Date { get; set; } = DateTime.Now;

        [ForeignKey("Parent_Category")]
        public int? Parent_Category_Id { get; set; }
        public Category? Parent_Category { get; set; }


        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
