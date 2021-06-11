using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Campaigns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Campaigns
{
    public class DiscountOrderMap : IEntityMappingConfiguration<DiscountOrder>
    {
        public void Map(EntityTypeBuilder<DiscountOrder> builder)
        {
            builder.ToTable("DiscountOrder");
        }
    }
}
