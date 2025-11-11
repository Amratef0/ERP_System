using ERP_System_Project.Models.ECommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class OrderStatusCodeConfig : IEntityTypeConfiguration<OrderStatusCode>
    {
        public void Configure(EntityTypeBuilder<OrderStatusCode> builder)
        {
            builder.HasData(
                new OrderStatusCode
                {
                    Id = 1,
                    Code = "PENDING",
                    Name = "Pending",
                    Description = "Order has been created but not yet processed."
                },
                new OrderStatusCode
                {
                    Id = 2,
                    Code = "PROCESSING",
                    Name = "Processing",
                    Description = "Order is being prepared and processed."
                },
                new OrderStatusCode
                {
                    Id = 3,
                    Code = "SHIPPED",
                    Name = "Shipped",
                    Description = "Order has been shipped and is on the way."
                },
                new OrderStatusCode
                {
                    Id = 4,
                    Code = "DELIVERED",
                    Name = "Delivered",
                    Description = "Order has been delivered to the customer."
                },
                new OrderStatusCode
                {
                    Id = 5,
                    Code = "CANCELLED",
                    Name = "Cancelled",
                    Description = "Order has been cancelled by the customer or admin."
                },
                new OrderStatusCode
                {
                    Id = 6,
                    Code = "RETURNED",
                    Name = "Returned",
                    Description = "Order has been returned by the customer."
                }
            );
        }
    }
}
