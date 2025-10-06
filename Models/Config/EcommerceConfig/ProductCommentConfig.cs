using ERP_System_Project.Models.ECommerce;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP_System_Project.Models.Config.EcommerceConfig
{
    public class ProductCommentConfig : IEntityTypeConfiguration<ProductComment>
    {
        public void Configure(EntityTypeBuilder<ProductComment> builder)
        {
            builder.Property(pc => pc.CommentDate).HasDefaultValueSql("GETDATE()");
        }
    }
}
