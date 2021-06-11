using Kimerce.Backend.Domain.WareHouse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Locations
{
    public class WareHouseMap : IEntityMappingConfiguration<WareHouse>
    {
        public void Map(EntityTypeBuilder<WareHouse> builder)
        {

        }
    }
}
