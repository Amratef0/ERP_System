using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

﻿using ERP_System_Project.Models.CRM;
using Microsoft.Identity.Client;

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

        [Required(ErrorMessage = "Order Number Is Required")]
        [StringLength(50, ErrorMessage = "Order Number Must Be Less Than 50 Characters")]
        public string OrderNumber { get; set; } = null!;

        [Required(ErrorMessage = "Order Date Is Required")]
        public DateTime OrderDate { get; set; }

        [ForeignKey("OrderStatusCode")]
        public int StatusId {  get; set; }
        public OrderStatusCode OrderStatusCode { get; set; }

        [DecimalPrecisionScale(15,4)]
        public decimal TotalAmount { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal SubTotalAmount { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal TaxAmount { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal ShippingAmount { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal DiscountAmount { get; set; } = 0;

        [ForeignKey("ShippingAddress")]
        public int? ShippingAddressId { get; set; }
        public CustomerAddress? ShippingAddress { get; set; }

        [ForeignKey("ShippingAddress")]
        public int BillingAddressId { get; set; }
        public CustomerAddress BillingAddress { get; set; }

        [ForeignKey("PaymentMethod")]
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        [ForeignKey("PaymentStatusCode")]
        public int PaymentStatusId { get; set; }
        public PaymentStatusCode PaymentStatusCode { get; set; }

        [ForeignKey("ShippingMethod")]
        public int ShippingMethodId { get; set; }
        public ShippingMethod ShippingMethod { get; set; }

        [StringLength(100, ErrorMessage = "Tracking Number Must Be Less Than 100 Characters")]
        public string? TrackingNumber { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }

        [StringLength(1000, ErrorMessage = "Notes Must Be Less Than 1000 Characters")]
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;


        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}
