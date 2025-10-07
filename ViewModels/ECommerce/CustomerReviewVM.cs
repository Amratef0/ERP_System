namespace ERP_System_Project.ViewModels.ECommerce
{
    public class CustomerReviewVM
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerImageURL { get; set; } = null!;
        public int Rate { get; set; } // 0 - 5 (stars)
        public string Comment { get; set; } = null!;
        public DateTime DateCreated { get; set; }
    }
}
