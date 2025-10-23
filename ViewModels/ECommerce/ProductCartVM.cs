namespace ERP_System_Project.ViewModels.ECommerce
{
    public class ProductCartVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageURL { get; set; } = null!;

        public decimal Price { get; set; }
        public bool HaveOffer { get; set; } = false;
        public int DiscountPercentage { get; set; } = 0;
        public decimal NetPrice { get; set; }

        public int Quantity { get; set; }
    }
}
