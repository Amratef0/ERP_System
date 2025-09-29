using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.Inventory
{
    public class BrandVM
    {
        public int Id { get; set; } 

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string LogoURL { get; set; } = null!;

        public string WebsiteURL { get; set; }
    }
}
