using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models
{
    public class ProductInventory
    {
        [Key]
        public int Inventory_Id { get; set; }

        [Range(0.01, 10_000_000_000, ErrorMessage = "Quantity On Hand must be greater than 0.")]
        [Precision(15, 4)]
        public decimal Quantity_On_Hand { get; set; } = 0;

        [Range(0.01, 10_000_000_000, ErrorMessage = "Quantity Committed must be greater than 0.")]
        [Precision(15, 4)]
        public decimal Quantity_Committed { get; set; } = 0;

        [Range(0.01, 10_000_000_000, ErrorMessage = "Quantity Available must be greater than 0.")]
        [Precision(15, 4)]
        public decimal Quantity_Available { get; set; } = 0;

        [Range(0.01, 10_000_000_000, ErrorMessage = "Quantity On Order must be greater than 0.")]
        [Precision(15, 4)]
        public decimal Quantity_On_Order { get; set; } = 0;
        public DateTime? Last_Count_Date { get; set; }
        public DateTime? Last_Receipt_Date { get; set; }
        public DateTime? Last_Issue_Date { get; set; }
        public DateTime Created_Date { get; set; } = DateTime.Now;
        public DateTime? Modified_Date { get; set; }


        [ForeignKey("Product")]
        public int Product_Id { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Warehouse")]
        public int Warehouse_Id { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
