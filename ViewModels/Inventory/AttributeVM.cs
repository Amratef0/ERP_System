using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.Inventory
{
    public class AttributeVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
