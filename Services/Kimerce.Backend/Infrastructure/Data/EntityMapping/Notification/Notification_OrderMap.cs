using Kimerce.Backend.Domain.Notification;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Orders;
using Microsoft.EntityFrameworkCore;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Notification
{
    public class Notification_OrderMap : IEntityMappingConfiguration<Notification_Order>
    {
        public void Map(EntityTypeBuilder<Notification_Order> builder)
        {

            builder.HasOne(no => no.notification).WithMany().HasForeignKey(or => or.NotiId);
            builder.HasOne(no => no.order).WithMany().HasForeignKey(no => no.OrderId);

            builder.ToTable("Notification_OrderItem");
        }

    }
}
