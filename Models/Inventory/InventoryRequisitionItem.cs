using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ERP_System_Project.Models.Inventory
{
    public class InventoryRequisitionItem
    {
        public int Id { get; set; }

        [ForeignKey("Inventoryrequestion")]
        public int RequestionId { get; set; }
        public InventoryRequisition Inventoryrequestion { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("ProductVariant")]
        public int VariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } 

        [DecimalPrecisionScale(15,4)]
        public decimal QuantityRequested { get; set; }

        [DecimalPrecisionScale(15, 4)]
        public decimal? QuantityApproved { get; set; }

        [StringLength(50, ErrorMessage = "Urgency Level Must Be Less Than 50 Characters")]
        public string? UrgencyLevel { get; set; }
        public DateTime NeededByDate { get; set; }

        [StringLength(1000, ErrorMessage = "Comments Must Be Less Than 1000 Characters")]
        public string? Comments { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
