using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Kimerce.Backend.Domain.Email;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Locations
{
    public class EmailMap : IEntityMappingConfiguration<Email>
    {
        public void Map(EntityTypeBuilder<Email> builder)
        {

        }
    }
}
