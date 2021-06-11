using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Kimerce.Backend.Domain.Email;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Locations
{
    public class EmailTemplateMap : IEntityMappingConfiguration<EmailTemplate>
    {
        public void Map(EntityTypeBuilder<EmailTemplate> builder)
        {
            builder.HasMany(t => t.Emails)
                .WithOne(e => e.Template)
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
