using Kimerce.Backend.Domain.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Notifications
{
    public class NotificationMap : IEntityMappingConfiguration<Domain.Notification.Notification>
    {
        public void Map(EntityTypeBuilder<Domain.Notification.Notification> builder)
        {
            builder.ToTable("Notification");
        }
    }
}
