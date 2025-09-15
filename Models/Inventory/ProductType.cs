using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Inventory
{
    public class ProductType
    {
        [Key]
        public int Type_Id { get; set; }
        public string? Type_Name { get; set; }
        public string? Description { get; set;}
        public bool Is_Active { get; set; } = true;

        public ICollection<Product> Products { get; set;} = new List<Product>();
    }
}
