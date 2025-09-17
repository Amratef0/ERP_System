using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.ECommerce
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name Is Required")]
        [StringLength(200, ErrorMessage = "Name Must Be Less Than 50 Characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Type Is Required")]
        [StringLength(50, ErrorMessage = "Type Must Be Less Than 50 Characters")]
        public string Type { get; set; } = null!;

        [DecimalPrecisionScale(10,2)]
        public decimal? DiscountValue { get; set; }
        public int? BuyQuantity { get; set; }
        public int? GetQuantity { get; set; }

        [DecimalPrecisionScale(15, 2)]
        public decimal? MinOrderAmount { get; set; }

        [StringLength(50, ErrorMessage = "Coupon Code Must Be Less Than 50 Characters")]
        public string? CouponCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;


        public ICollection<OfferProduct> ProductsOffer { get; set; } = new List<OfferProduct>();
        public ICollection<OfferCategory> CategoriesOffer { get; set; } = new List<OfferCategory>();
    }
}
