using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models
{
    public class InventoryTransactionType
    {
        [Key]
        public int Type_Id { get; set; }

        [Required(ErrorMessage = "Type Code Is Required")]
        [StringLength(50, ErrorMessage = "Type Code Must Be Less Than 50 Characters")]
        public string Type_Code { get; set; } = null!;

        [Required(ErrorMessage = "Type_Name Is Required")]
        [StringLength(100, ErrorMessage = "Type Name Must Be Less Than 100 Characters")]
        public string Type_Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Is_Active { get; set; } = true;
    }
}
