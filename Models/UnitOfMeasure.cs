using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models
{
    public class UnitOfMeasure
    {
        [Key]
        public int UOM_Id { get; set; }
        public string? UOM_Type { get; set; }
        public string? UOM_Code { get; set; }
        public string? UOM_Name { get; set; }
        public bool Is_Active { get; set; } = true;

        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
