using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Inventory
{
    public class WarehouseProduct
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }

        public decimal QuantityOnHand { get; set; } = 0;
    }
}
