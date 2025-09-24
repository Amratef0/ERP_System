using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.CRMConfig
{
    public class CustomerWishlistConfiguration : IEntityTypeConfiguration<CustomerWishlist>
    {
        public void Configure(EntityTypeBuilder<CustomerWishlist> builder)
        {

            builder.HasKey(w => w.Id);
        }
    }
}
