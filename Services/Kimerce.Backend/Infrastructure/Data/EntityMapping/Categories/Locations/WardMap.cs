using Kimerce.Backend.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Locations
{
    public class WardMap : IEntityMappingConfiguration<Ward>
    {
        public void Map(EntityTypeBuilder<Ward> builder)
        {
            builder.HasMany(w => w.WareHouses)
                   .WithOne(w => w.Ward)
                   .HasForeignKey(w => w.ParentId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
