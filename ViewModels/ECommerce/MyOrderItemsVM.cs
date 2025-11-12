namespace ERP_System_Project.ViewModels.ECommerce
{
    public class MyOrderItemsVM
    {
        public string ProductName { get; set; }
        public string ProductImagePath { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal LineTotal { get; set; }
    }
}
