using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.Inventory;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.CRM
{
    public class CustomerReviewVM
    {
        public int Id { get; set; }



        [Required(ErrorMessage = "Comment is required")]
        [StringLength(250, ErrorMessage = "Comment cannot exceed 250 characters")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public decimal Rating { get; set; } // e.g., 1 to 5

        [Required(ErrorMessage = "Customer is required")]
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; }
        public bool IsEdited { get; set; } = false;
        [DataType(DataType.DateTime)]
        public DateTime? EditedAt { get; set; }

    }
}
