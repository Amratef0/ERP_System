using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Inventory
{
    public class UnitOfMeasure
    {
        [Key]
        public int UOM_Id { get; set; }

        [Required(ErrorMessage = "Type of Unit Of Measure Is Requierd")]
        [StringLength(50, ErrorMessage = "Type of Unit Of Measure Must Be Less Than 50 Characters")]
        public string UOM_Type { get; set; } = null!;

        [Required(ErrorMessage = "Code of Unit Of Measure Is Requierd")]
        [StringLength(100, ErrorMessage = "Code of Unit Of Measure Must Be Less Than 100 Characters")]
        public string UOM_Code { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Name of Unit Of Measure Must Be Less Than 500 Characters")]
        public string? UOM_Name { get; set; }
        public bool Is_Active { get; set; } = true;

        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
