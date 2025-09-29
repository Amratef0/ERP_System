using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.ValidationAttributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Inventory
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal AdditionalPrice { get; set; } = 0;

        [DecimalPrecisionScale(15, 4)] 
        public int Quantity { get; set; } = 0;

        [ImageFile]
        public string ImageURL { get; set; } = null!;
        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }


        public ICollection<VariantAttributeValue> ProductAttributes { get; set; } = new List<VariantAttributeValue>();
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
        public ICollection<InventoryRequisitionItem> InventoryRequestedVariantProducts { get; set; } = new List<InventoryRequisitionItem>();
        public ICollection<OrderItem> OrderedItems { get; set; } = new List<OrderItem>();
        public ICollection<OfferProduct> Offers { get; set; } = new List<OfferProduct>();

    }
}
