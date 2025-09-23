using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class LeaveTypeConfig : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder.HasIndex(lt => lt.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(lt => lt.IsPaid)
                   .HasDefaultValue(true);

            builder.Property(lt => lt.IsActive)
                   .HasDefaultValue(true);

            builder.Property(lt => lt.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(lt => !lt.IsDeleted);
        }
    }
}
