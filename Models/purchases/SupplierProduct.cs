using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_System_Project.Models.Inventory;

namespace ERP_System_Project.Models
{
    public class SupplierProduct
    {
        [Key]
        public int SupplierProductId { get; set; }

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Column(TypeName = "decimal(15,4)")]
        public decimal UnitPrice { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
