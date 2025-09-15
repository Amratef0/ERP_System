using ERP_System_Project.Models.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ERP_System_Project.Models
{
    //work
    public class Warehouse
    {
        [Key]
        public int Warehouse_Id { get; set; }

        [Required(ErrorMessage = "Warehouse Code Is Required")]
        [StringLength(50, ErrorMessage = "Warehouse Code Must be less than 50 characters")]
        public string Warehouse_Code { get; set; } = null!;

        [Required(ErrorMessage = "Warehouse Name Is Required")]
        [StringLength(100, ErrorMessage = "Warehouse Name Must be less than 100 characters")]
        public string Warehouse_Name { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Warehouse Type Must be less than 50 characters")]
        public string? Warehouse_Type { get; set; }
        public bool Is_Active { get; set; } = true;
        public DateTime Created_Date { get; set; } = DateTime.Now;

        [ForeignKey("Branch")]
        public int Branch_Id { get; set; }
        public Branch Branch { get; set; }

        [ForeignKey("Address")]
        public int? Address_Id { get; set; }
        public Address? Address { get; set; }


        public ICollection<ProductInventory> Products { get; set; } = new List<ProductInventory>();


    }
}
