using Microsoft.AspNetCore.Identity;

namespace ERP_System_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
       public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
