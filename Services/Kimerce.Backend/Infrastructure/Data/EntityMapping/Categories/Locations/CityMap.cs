using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Domain.Locations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Locations
{
    public class CityMap : IEntityMappingConfiguration<City>
    {
        public void Map(EntityTypeBuilder<City> builder)
        {
            builder.HasMany(c => c.Districts)
                   .WithOne(d => d.City)
                   .HasForeignKey(d => d.ParentId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}