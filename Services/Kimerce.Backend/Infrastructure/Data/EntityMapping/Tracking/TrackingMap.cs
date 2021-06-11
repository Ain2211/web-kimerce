using Kimerce.Backend.Domain.Tracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Trackings
{
    public class TrackingMap : IEntityMappingConfiguration<Tracking>
    {
        public void Map(EntityTypeBuilder<Tracking> builder)
        {
            builder.ToTable("Tracking");
        }
    }
}
