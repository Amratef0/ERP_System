using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Inventory
{
    public class InventoryTransactionType
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Type Code Is Required")]
        [StringLength(50, ErrorMessage = "Type Code Must Be Less Than 50 Characters")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "Type_Name Is Required")]
        [StringLength(100, ErrorMessage = "Type Name Must Be Less Than 100 Characters")]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;


        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    }
}
