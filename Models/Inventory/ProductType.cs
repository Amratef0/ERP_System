using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Inventory
{
    public class ProductType
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set;}
        public bool IsActive { get; set; } = true;

        public ICollection<Product> Products { get; set;} = new List<Product>();
    }
}
