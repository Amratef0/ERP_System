using System.Data;
using System.Xml.Linq;

namespace ERP_System_Project.Models.Inventory
{
    public class InventoryRequisition
    {
    }

    //requisition_id INT PRIMARY KEY IDENTITY(1,1),
    //requisition_number NVARCHAR(50) UNIQUE NOT NULL,
    //warehouse_id INT NOT NULL,
    //requested_by_employee_id INT NOT NULL,
    //requisition_date DATE NOT NULL,
    //status_id INT NOT NULL,
    //approved_by_employee_id INT NULL,
    //approved_date DATE NULL,
    //comments NVARCHAR(1000),
    //created_date DATETIME2 DEFAULT GETDATE(),
    //modified_date DATETIME2 DEFAULT GETDATE(),
    //CONSTRAINT fk_requisitions_warehouse FOREIGN KEY(warehouse_id) REFERENCES warehouses(warehouse_id),
    //CONSTRAINT fk_requisitions_requested_by FOREIGN KEY(requested_by_employee_id) REFERENCES employees(employee_id),
    //CONSTRAINT fk_requisitions_approved_by FOREIGN KEY(approved_by_employee_id) REFERENCES employees(employee_id),
    //CONSTRAINT fk_requisitions_status FOREIGN KEY(status_id) REFERENCES inventory_requisition_status_codes(status_id)

}
