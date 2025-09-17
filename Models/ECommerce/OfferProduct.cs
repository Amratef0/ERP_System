using ERP_System_Project.Models.Inventory;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.ECommerce
{
    public class OfferProduct
    {
        [ForeignKey("Offer")]
        public int OfferId { get; set; }
        public Offer Offer { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("ProductVariant")]
        public int? VariantId { get; set; }
        public ProductVariant? ProductVariant { get; set; }
    }
}
