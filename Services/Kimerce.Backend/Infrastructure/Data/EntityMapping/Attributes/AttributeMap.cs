using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Attributes
{
    public class AttributeMap : IEntityMappingConfiguration<Attribute>
    {
        public void Map(EntityTypeBuilder<Attribute> builder)
        {
            builder.ToTable("Attribute");
        }
    }
}
