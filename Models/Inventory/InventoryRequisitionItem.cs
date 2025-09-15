using System.Data;

namespace ERP_System_Project.Models.Inventory
{
    public class InventoryRequisitionItem
    {
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
