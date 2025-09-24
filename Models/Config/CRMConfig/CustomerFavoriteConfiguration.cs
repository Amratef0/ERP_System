using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.CRMConfig
{
    public class CustomerFavoriteConfiguration : IEntityTypeConfiguration<CustomerFavorite>
    {
        public void Configure(EntityTypeBuilder<CustomerFavorite> builder)
        {



            builder.HasKey(f => f.Id);


            builder.Property(f => f.DateCreated)
                   .HasDefaultValueSql("GETDATE()")
                   .ValueGeneratedOnAdd();


        }
    }
}
