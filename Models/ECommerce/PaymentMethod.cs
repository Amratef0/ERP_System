using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.ECommerece;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ERP_System_Project.Models.ECommerce
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PaymentMethodType")]
        public int PaymentTypeId { get; set; }
        public PaymentMethodType PaymentMethodType { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer {get; set; }

        [StringLength(4, MinimumLength = 4, ErrorMessage = "Please enter 4 numbers")]
        public string? CardLast4 { get; set; }

        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;


        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
