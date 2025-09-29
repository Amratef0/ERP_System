using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.CRM
{
    public class CustomerTypeVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Type Name is required")]
        [StringLength(100, ErrorMessage = "Type Name must be at most 100 characters")]
        [Display(Name = "Type Name")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Description must be at most 500 characters")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Type Description")]

        public string Description { get; set; }

        [Display(Name = "Number of Customers")]
        public int CustomerCount { get; set; }
    }
}
