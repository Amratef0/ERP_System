using ERP_System_Project.Models.CRM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.CRMConfig
{
    public class CustomerTypeConfiguration : IEntityTypeConfiguration<CustomerType>
    {
        public void Configure(EntityTypeBuilder<CustomerType> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Description).IsRequired();

            builder.HasData(
                new CustomerType
                {
                    Id = 1,
                    Name = "Standard",
                    Description = "Regular customer with no permanent discounts. Eligible for general promotions and seasonal offers."
                },
                new CustomerType
                {
                    Id = 2,
                    Name = "Silver Member",
                    Description = "Receives a 2% discount on all products after purchasing goods worth $1,000 in total or 10 items across three orders."
                },
                new CustomerType
                {
                    Id = 3,
                    Name = "Gold Member",
                    Description = "Receives a 5% discount on all products after purchasing goods worth $5,000 in total or 25 items across ten orders."
                },
                new CustomerType
                {
                    Id = 4,
                    Name = "Platinum Member",
                    Description = "Receives a 10% discount on all products after purchasing goods worth $15,000 in total or 50 items across fifty orders."
                },
                new CustomerType
                {
                    Id = 5,
                    Name = "Diamond Elite",
                    Description = "Receives a 15% discount on all products after purchasing goods worth $30,000 in total or 100 items across one hundred orders, along with free shipping and priority customer service."
                }
            );
        }
    }
}
