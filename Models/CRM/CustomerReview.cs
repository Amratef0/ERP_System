using ERP_System_Project.Models.Inventory;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.CRM
{
    public class CustomerReview
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } // e.g., 1 to 5
        public  int? CustomerId{ get; set; }
        public  Customer? Customer{ get; set; }
        public  int? ProductId{ get; set; }
        public  Product? Product{ get; set; }

    }
}
