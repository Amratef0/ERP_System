using ERP_System_Project.Models.Inventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.ViewModels.Inventory
{
    public class CategoryVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name Is Required")]
        [StringLength(50, ErrorMessage = "Category Name Must Be Less Than 50 Characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Category Description Is Required")]
        public string Description { get; set; } = null!;
    }
}
