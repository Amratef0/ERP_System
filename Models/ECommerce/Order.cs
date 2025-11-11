using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.ECommerece
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey("Currency")]
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        [Required(ErrorMessage = "Order Date Is Required")]
        public DateTime OrderDate { get; set; }

        [ForeignKey("OrderStatusCode")]
        public int StatusId {  get; set; }
        public OrderStatusCode OrderStatusCode { get; set; }

        [DecimalPrecisionScale(15,4)]
        public decimal TotalAmount { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal TaxAmount { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal ShippingAmount { get; set; }

        [ForeignKey("BillingAddress")]
        public int BillingAddressId { get; set; }
        public CustomerAddress BillingAddress { get; set; }

        [ForeignKey("PaymentMethodType")]
        public int? PaymentMethodTypeId { get; set; }
        public PaymentMethodType? PaymentMethodType { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }

        [StringLength(1000, ErrorMessage = "Notes Must Be Less Than 1000 Characters")]
        public string? Notes { get; set; }
        public DateTime? ModifiedDate { get; set; }


        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
