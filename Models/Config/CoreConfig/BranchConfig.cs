using ERP_System_Project.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.CoreConfig
{
    public class BranchConfig : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasIndex(b => b.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(b => b.IsMainBranch).HasDefaultValue(false);
            builder.Property(b => b.IsActive).HasDefaultValue(true);

            builder.Property(b => b.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(b => !b.IsDeleted);

            // Configure relationships with restrict delete behavior
            builder.HasOne(b => b.Address)
                   .WithMany(a => a.Branches)
                   .HasForeignKey(b => b.AddressId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
