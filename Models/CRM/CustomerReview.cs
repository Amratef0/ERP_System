using ERP_System_Project.Models.Inventory;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.CRM
{
    public class CustomerReview
    {
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Comment { get; set; }

        [Range(1, 5)]
        public decimal Rating { get; set; } // e.g., 1 to 5
        public  int CustomerId{ get; set; }
        public  Customer? Customer{ get; set; }
        public  int ProductId{ get; set; }
        public  Product? Product{ get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsEdited { get; set; } = false;
        [DataType(DataType.DateTime)]
        public DateTime? EditedAt { get; set; }

    }
}
