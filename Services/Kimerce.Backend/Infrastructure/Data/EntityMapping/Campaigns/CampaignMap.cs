using Kimerce.Backend.Domain.Campaigns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Campaigns
{
    public class CampaignMap : IEntityMappingConfiguration<Campaign>
    {
        public void Map(EntityTypeBuilder<Campaign> builder)
        {
            builder.ToTable("Campaign");
            builder.HasMany(w => w.CampaignOrders)
                   .WithOne(w => w.Campaign)
                   .HasForeignKey(w => w.CampaignId)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(w => w.CampaignProducts)
                   .WithOne(w => w.Campaign)
                   .HasForeignKey(w => w.CampaignId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}