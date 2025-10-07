namespace ERP_System_Project.ViewModels.ECommerce
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageURL { get; set; } = null!;

        public decimal Price { get; set; }
        public bool HasOffer { get; set; } = false;
        public int TotalOffer { get; set; } = 0;
        public decimal NetPrice { get; set; }

        public int TotalRate { get; set; } // 0 - 5 (stars)
        public int NumberOfReviewers { get; set; }
        public List<CustomerReviewVM> Reviews { get; set; } = new List<CustomerReviewVM>();

        public string BrandName { get; set; } = null!;
        public string BrandImageURL { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public int Quantity { get; set; } = 1;
    }
}
