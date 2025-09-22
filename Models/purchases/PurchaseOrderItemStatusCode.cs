using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models
{
    public class PurchaseOrderItemStatusCode
    {
        [Key]
        public int ItemStatusId { get; set; }

        [Required, StringLength(50)]
        public string StatusCode { get; set; }

        [Required, StringLength(100)]
        public string StatusName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    }
}
