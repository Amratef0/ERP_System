using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.Inventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.ECommerce
{
    public class ProductRating
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Rate { get; set; }
        public DateTime RateDate { get; set; } = DateTime.Now;
    }
}
