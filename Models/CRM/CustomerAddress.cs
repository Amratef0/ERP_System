using ERP_System_Project.Models.ECommerece;

namespace ERP_System_Project.Models.CRM
{
    public class CustomerAddress
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;


        public ICollection<Order> OrderShippingAddresses { get; set; } = new List<Order>();
        public ICollection<Order> OrderBillingAddresses { get; set; } = new List<Order>();
    }
}
