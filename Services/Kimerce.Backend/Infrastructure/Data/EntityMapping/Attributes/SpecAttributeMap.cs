using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Attributes
{
    public class SpecAttributeMap : IEntityMappingConfiguration<SpecAttribute>
    {
        public void Map(EntityTypeBuilder<SpecAttribute> builder)
        {
            
        }
    }
}
