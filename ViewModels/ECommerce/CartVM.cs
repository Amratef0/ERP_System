namespace ERP_System_Project.ViewModels.ECommerce
{
    public class CartVM
    {
        public decimal TotalPrice { get; set; }
        public List<ProductCartVM> productsCart { get; set; } = new List<ProductCartVM>();
    }
}
