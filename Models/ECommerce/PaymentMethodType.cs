using ERP_System_Project.Models.ECommerece;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.ECommerce
{
    public class PaymentMethodType
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Payment Method Type Is Required")]
        [StringLength(50, ErrorMessage = "Payment Method Type Must Be Less Than 50 Characters")]
        public string Type { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Provider Must Be Less Than 50 Characters")]
        public string? Provider { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
