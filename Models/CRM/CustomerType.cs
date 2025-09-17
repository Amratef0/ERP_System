namespace ERP_System_Project.Models.CRM
{
    public class CustomerType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       public ICollection<Customer> Customers { get; set; } = new HashSet<Customer>();
    }
}
