using Kimerce.Backend.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping
{
    public class OrderMap : IEntityMappingConfiguration<Kimerce.Backend.Domain.Orders.Order>
    {

        public void Map(EntityTypeBuilder<Domain.Orders.Order> builder)
        {
            builder.ToTable("Order");
        }
    }
}
