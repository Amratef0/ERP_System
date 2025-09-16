using ERP_System_Project.Models.HR;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Xml.Linq;

namespace ERP_System_Project.Models.Inventory
{
    public class InventoryRequisition
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Requestion Number Is Required")]
        [StringLength(100, ErrorMessage = "Requestion Number Must Be Less Than 50 Characters")]
        public string Number { get; set; } = null!;

        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        [ForeignKey("RequestingEmployee")]
        public int RequestedByEmployeeId { get; set; } 
        public Employee RequestingEmployee { get; set; }
        public DateTime RequestionDate { get; set; }

        [ForeignKey("RequstionStatusCode")]
        public int StatusId { get; set; }
        public InventoryRequisitionStatusCode RequstionStatusCode { get; set; }

        [ForeignKey("ApprovingEmployee")]
        public int? ApprovedByEmployeeId { get; set; }
        public Employee? ApprovingEmployee { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [StringLength(1000, ErrorMessage = "Comments Must Be Less Than 1000 Characters")]
        public string? Comments { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;




        public ICollection<InventoryRequisitionItem> RequestedItems { get; set; } = new List<InventoryRequisitionItem>();


    }
}
