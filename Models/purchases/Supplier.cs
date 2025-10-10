using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required, StringLength(50)]
        public string? SupplierCode { get; set; }

        [Required, StringLength(255)]
        public string? SupplierName { get; set; }

        public int SupplierCategoryId { get; set; }
        public SupplierCategory? SupplierCategory { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;

        public ICollection<SupplierProduct>? SupplierProducts { get; set; }
        public ICollection<PurchaseOrder>? PurchaseOrders { get; set; }
    }
}
