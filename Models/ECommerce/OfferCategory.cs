using ERP_System_Project.Models.Inventory;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ERP_System_Project.Models.ECommerce
{
    public class OfferCategory
    {
        [ForeignKey("Offer")]
        public int OfferId { get; set; }
        public Offer Offer { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
