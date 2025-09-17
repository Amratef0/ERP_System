using ERP_System_Project.Models.CRM;
using Microsoft.Identity.Client;

namespace ERP_System_Project.Models.ECommerece
{
    public class Order
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}
