using ERP_System_Project.Models.ECommerece;

namespace ERP_System_Project.Models.CRM
{
    public class CustomerAddress
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public string Country { get; set; }
        public string City { get; set; }
        public string Street{ get; set; }
        public int BuildingNumber{ get; set; }
        public int ApartmentNumber{ get; set; }

        public ICollection<Order> OrderShippingAddresses { get; set; } = new List<Order>();
        public ICollection<Order> OrderBillingAddresses { get; set; } = new List<Order>();
    }
}
