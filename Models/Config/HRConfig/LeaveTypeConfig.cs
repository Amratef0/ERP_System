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

            builder.HasData(
                new LeaveType
                {
                    Id = 1,
                    Name = "Annual Leave",
                    Code = "ANNUAL_LEAVE",
                    MaxDaysPerYear = 21,
                    IsPaid = true,
                    IsActive = true,
                    IsDeleted = false
                },
                new LeaveType
                {
                    Id = 2,
                    Name = "Sick Leave",
                    Code = "SICK_LEAVE",
                    MaxDaysPerYear = 14,
                    IsPaid = true,
                    IsActive = true,
                    IsDeleted = false
                },
                new LeaveType
                {
                    Id = 3,
                    Name = "Casual Leave",
                    Code = "CASUAL_LEAVE",
                    MaxDaysPerYear = 7,
                    IsPaid = true,
                    IsActive = true,
                    IsDeleted = false
                },
                new LeaveType
                {
                    Id = 4,
                    Name = "Maternity Leave",
                    Code = "MATERNITY_LEAVE",
                    MaxDaysPerYear = 90,
                    IsPaid = true,
                    IsActive = true,
                    IsDeleted = false
                },
                new LeaveType
                {
                    Id = 5,
                    Name = "Unpaid Leave",
                    Code = "UNPAID_LEAVE",
                    MaxDaysPerYear = null,
                    IsPaid = false,
                    IsActive = true,
                    IsDeleted = false
                }
            );
        }
    }
}