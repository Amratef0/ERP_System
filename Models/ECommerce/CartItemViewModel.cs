namespace ERP_System_Project.Models.ECommerce
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
