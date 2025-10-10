namespace ERP_System_Project.ViewModels.ECommerce
{
    public class ProductCardVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageURL { get; set; } = null!;

        public decimal Price { get; set; }
        public bool HaveOffer { get; set; } = false;
        public int DiscountPercentage { get; set; } = 0;
        public decimal NetPrice { get; set; }

        public int TotalRate { get; set; } // 0 - 5 (stars)
        public int NumberOfReviews { get; set; }

        public string BrandName { get; set; } = null!;
        public string BrandImageURL { get; set; } = null!;


        public string CategoryName { get; set; } = null!;
    }
}
