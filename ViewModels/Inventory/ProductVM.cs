using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.ViewModels.Inventory
{
    public class ProductVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        [DecimalPrecisionScale(10, 2)]
        [DisplayName("Unit Cost")]
        public decimal UnitCost { get; set; } = 0;

        [DisplayName("Standard Price")]
        public decimal StandardPrice { get; set; }

        [DisplayName("Brand")]
        public int BrandId { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }

        public int OfferPercentege { get; set; } = 0;
        public int Quantity { get; set; }

        [ImageFile]
        public IFormFile Image { get; set; } = null!;
        public string? ImageURL { get; set; }

        public List<ProductAttributeValueVM> AttributesVM { get; set; } = new List<ProductAttributeValueVM>();

    }
}
