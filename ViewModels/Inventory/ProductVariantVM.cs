using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.Inventory
{
    public class ProductVariantVM
    {
        [DecimalPrecisionScale(15, 4)]
        public decimal AdditionalPrice { get; set; } = 0;

        public int Quantity { get; set; } = 0;

        [DisplayName("Is Default")]
        public bool IsDefault { get; set; } = false;

        [ImageFile]
        public IFormFile Image { get; set; } = null!;

        [DisplayName("Atrribute")]
        [Required(ErrorMessage = "Attribute Option Is Requierd")]
        public int AtrributeId { get; set; }

        [Required(ErrorMessage = "Attribute Value Is Requierd")]
        [DisplayName("Atrribute Value")]
        [StringLength(255, ErrorMessage = "Attribute Value Must Be Less Than 255 Characters")]
        public string AtrributeValue { get; set; } = null!;
    }
}
