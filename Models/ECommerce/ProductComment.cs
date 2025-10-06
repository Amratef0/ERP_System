using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.Inventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.ECommerce
{
    public class ProductComment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [StringLength(300)]
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; } = DateTime.Now; 
    }
}
