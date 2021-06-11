using Kimerce.Backend.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Locations
{
    public class DistrictMap : IEntityMappingConfiguration<District>
    {
        public void Map(EntityTypeBuilder<District> builder)
        {
            builder.HasMany(d => d.Wards)
                   .WithOne(w => w.District)
                   .HasForeignKey(w => w.ParentId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
