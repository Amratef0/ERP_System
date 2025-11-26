namespace ERP_System_Project.Models.ECommerce
{
    public class CartViewModel
    {
        public List<CartItemViewModel> productsCart { get; set; } = new List<CartItemViewModel>();

        public decimal TotalPrice => productsCart.Sum(p => p.Quantity * p.Price);
    }
}
