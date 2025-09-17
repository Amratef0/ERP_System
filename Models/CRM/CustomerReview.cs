using ERP_System_Project.Models.Inventory;

namespace ERP_System_Project.Models.CRM
{
    public class CustomerReview
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public  int? CustomerId{ get; set; }
        public  Customer? Customer{ get; set; }
        public  int? ProductId{ get; set; }
        public  Product? Product{ get; set; }

    }
}
