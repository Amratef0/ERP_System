using ERP_System_Project.Models.Inventory;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.ViewModels.ECommerce
{
    public class OfferVM
    {
        public int ProductId { get; set; }
        public bool IsHasOffer { get; set; }
        public string Name { get; set; } = null!;
        public int DiscountPercentage { get; set; }
        public int OfferDays { get; set; }
        public DateTime StartDate { get; set; }
    }
}
