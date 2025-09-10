using ERP_System_Project.Models.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Models
{
    public class Erpdbcontext : IdentityDbContext<ApplicationUser>
    {
        public Erpdbcontext(DbContextOptions<Erpdbcontext> options) : base(options) { }



        #region Ecommerce&Inventory
        public DbSet<Brand> Brands { get; set; } 
        #endregion


        public DbSet<Order> Orders { get; set; }
    }
}