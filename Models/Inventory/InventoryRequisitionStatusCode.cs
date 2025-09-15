using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Inventory
{
    public class InventoryRequisitionStatusCode
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Status Code is required")]
        [StringLength(50, ErrorMessage = "Status Code Must Be Less Than 50 Characters")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "Status Name is required")]
        [StringLength(100, ErrorMessage = "Status Name Type Must Be Less Than 100 Characters")]
        public string Name { get; set;} = null!;

        [StringLength(255, ErrorMessage = "Description Must Be Less Than 255 Characters")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
