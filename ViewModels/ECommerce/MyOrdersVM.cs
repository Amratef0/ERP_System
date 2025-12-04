using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.ViewModels.ECommerce
{
    public class MyOrdersVM
    {
        public int OrderId { get; set; }

        public string? OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public string? BillingAddress { get; set; }
        public string? PaymentMethodType { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }


        public List<MyOrderItemsVM> OrderItemsVMs { get; set; } = new List<MyOrderItemsVM>();
        
    }
}
