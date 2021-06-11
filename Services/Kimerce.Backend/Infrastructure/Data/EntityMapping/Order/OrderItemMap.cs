using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping
{
    public class OrderItemMap : IEntityMappingConfiguration<Kimerce.Backend.Domain.Orders.Order_Item>
    {
        public void Map(EntityTypeBuilder<Order_Item> builder)
        {

            builder.HasOne(or => or.Order).WithMany().HasForeignKey(or => or.IdOrder);
            builder.HasOne(or => or.Product).WithMany().HasForeignKey(or => or.IdProduct);

            builder.ToTable("OrderItem");
        }
    }
}
