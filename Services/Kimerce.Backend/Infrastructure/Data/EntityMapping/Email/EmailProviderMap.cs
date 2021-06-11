using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Kimerce.Backend.Domain.Email;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Locations
{
    public class EmailProviderMap : IEntityMappingConfiguration<EmailProvider>
    {
        public void Map(EntityTypeBuilder<EmailProvider> builder)
        {
            builder.HasMany(p => p.Accounts)
                .WithOne(a => a.Provider)
                .HasForeignKey(a => a.ProviderId)
                .OnDelete(DeleteBehavior.Cascade);               
        }
    }
}
