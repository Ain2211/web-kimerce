using Kimerce.Backend.Domain.Shipments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Shipments
{
    public class ShipmentMap : IEntityMappingConfiguration<Shipment>
    {
        public void Map(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("Shipment");
        }
    }
}
