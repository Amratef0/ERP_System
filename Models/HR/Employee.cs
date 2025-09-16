using ERP_System_Project.Models.Inventory;

namespace ERP_System_Project.Models.HR
{
    public class Employee
    {




        
        
        
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
        public ICollection<InventoryRequisition> RequestedRequisitions { get; set; } = new List<InventoryRequisition>();
        public ICollection<InventoryRequisition> ApprovedRequisitions { get; set; } = new List<InventoryRequisition>();
    }
}
