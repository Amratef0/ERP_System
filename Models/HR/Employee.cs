using ERP_System_Project.Models.Inventory;

namespace ERP_System_Project.Models.HR
{
    public class Employee
    {




        
        
        
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    }
}
