using ERP_System_Project.Models.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ERP_System_Project.Models.Inventory
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Warehouse Code Is Required")]
        [StringLength(50, ErrorMessage = "Warehouse Code Must be less than 50 characters")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "Warehouse Name Is Required")]
        [StringLength(100, ErrorMessage = "Warehouse Name Must be less than 100 characters")]
        public string Name { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Warehouse Type Must be less than 50 characters")]
        public string? Type { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }


        public ICollection<ProductInventory> Products { get; set; } = new List<ProductInventory>();
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
        public ICollection<InventoryRequisition> InventoryRequisitions { get; set; } = new List<InventoryRequisition>();




    }
}
