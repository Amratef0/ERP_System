using ERP_System_Project.Models.Inventory;

namespace ERP_System_Project.Models.CRM
{
    public class CustomerWishlist
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
