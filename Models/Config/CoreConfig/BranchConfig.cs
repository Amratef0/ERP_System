using ERP_System_Project.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.CoreConfig
{
    public class BranchConfig : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.Property(b => b.IsMainBranch).HasDefaultValue(false);
            builder.Property(b => b.IsMainBranch).HasDefaultValue(true);
        }
    }
}
