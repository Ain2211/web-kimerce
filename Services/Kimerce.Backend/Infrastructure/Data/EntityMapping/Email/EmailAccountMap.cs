using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Kimerce.Backend.Domain.Email;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Locations
{
    public class EmailAccountMap : IEntityMappingConfiguration<EmailAccount>
    {
        public void Map(EntityTypeBuilder<EmailAccount> builder)
        {
            builder.HasMany(a => a.Emails)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
