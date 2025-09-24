using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.CRMConfig
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.Email).IsUnique();

            builder.Property(c => c.RegistrationDate)
                  .HasDefaultValueSql("GETDATE()")
                  .ValueGeneratedOnAdd()
                  .IsRequired();

            builder.Property(c => c.LastLoginDate)
                   .IsRequired(false);

            builder.Property(c => c.IsActive)
                  .HasDefaultValue(true)
                  .IsRequired();


         

            builder.Property(c => c.ModifiedDate)
                   .IsRequired(false);


            builder.Property(c => c.DeactivatedAt).IsRequired(false);


            // Relationships---------------------------------------------
            builder.HasOne(c => c.ApplicationUser)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.CustomerType)
                .WithMany(t => t.Customers)
                .HasForeignKey(c => c.CustomerTypeId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasMany(c => c.CustomerAddresses)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.CustomerFavorites)
                .WithOne(f=> f.Customer)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // soft delete to preserve order history

            //shopping cart and payment methods will be added later





            builder.HasMany(c => c.Reviews)
                 .WithOne(r => r.Customer)
                 .HasForeignKey(r => r.CustomerId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Wishlists)
                   .WithOne(w => w.Customer)
                   .HasForeignKey(w => w.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
