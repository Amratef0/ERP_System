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


 //   requisition_item_id INT PRIMARY KEY IDENTITY(1,1),
 //   requisition_id INT NOT NULL,
 //   product_id INT NOT NULL,
 //   variant_id INT NOT NULL,
 //   quantity_requested DECIMAL(15,4) NOT NULL,
 //   quantity_approved DECIMAL(15,4) NULL,
 //   urgency_level NVARCHAR(50),
 //   needed_by_date DATE,
 //   comments NVARCHAR(1000),
 //   created_date DATETIME2 DEFAULT GETDATE(),
 //   CONSTRAINT fk_requisition_items_requisition FOREIGN KEY(requisition_id) REFERENCES inventory_requisitions(requisition_id),
 //   CONSTRAINT fk_requisition_items_product FOREIGN KEY(product_id) REFERENCES products(product_id),
 //   CONSTRAINT fk_requisition_items_variant FOREIGN KEY(variant_id) REFERENCES product_variants(variant_id)

}
