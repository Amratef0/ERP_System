using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.ECommerece;
using Microsoft.AspNetCore.Identity;

namespace ERP_System_Project.Models
{
    public class ApplicationUser : IdentityUser
    {

        // Navigation property to Customer (one -to-one relationship) ApplicationUser can have only one Customer profile
        public virtual Customer? Customer { get; set; }
    }
}
