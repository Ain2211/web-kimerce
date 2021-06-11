using Kimerce.Backend.Domain.Campaigns;
using Kimerce.Backend.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Campaigns
{
    public class DiscountMap : IEntityMappingConfiguration<Discount>
    {
        public void Map(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable("Discount");
            builder.HasMany(w => w.DiscountOrders)
                   .WithOne(w => w.Discount)
                   .HasForeignKey(w => w.DiscountId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}