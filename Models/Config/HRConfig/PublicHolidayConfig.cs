using ERP_System_Project.Models.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.HRConfig
{
    public class PublicHolidayConfig : IEntityTypeConfiguration<PublicHoliday>
    {
        public void Configure(EntityTypeBuilder<PublicHoliday> builder)
        {
            builder.HasIndex(ph => new { ph.CountryId, ph.Date })
               .HasFilter("[IsDeleted] = 0");

            builder.HasIndex(ph => ph.IsDeleted)
                   .HasFilter("[IsDeleted] = 0");

            builder.Property(ph => ph.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(ph => !ph.IsDeleted);

            // Configure relationships with restrict delete behavior
            builder.HasOne(ph => ph.Country)
                   .WithMany()
                   .HasForeignKey(ph => ph.CountryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}