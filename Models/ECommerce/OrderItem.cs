using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.ECommerce
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; } 

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("ProductVariant")]
        public int? VariantId { get; set; }
        public ProductVariant? ProductVariant { get; set; }

        [DecimalPrecisionScale(15,4)]
        public decimal Quantity { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal UnitPrice { get; set; }

        [DecimalPrecisionScale(5, 2)]
        public decimal DiscountPercentage { get; set; } = 0;

        [DecimalPrecisionScale(15, 4)]
        public decimal DiscountAmount { get; set; } = 0;

        [DecimalPrecisionScale(15, 4)]
        public decimal LineTotal { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal TaxAmount { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
