using Kimerce.Backend.Domain.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Data.EntityMapping.Notification
{
    public class Notification_ImageMap
    {
        public void Map(EntityTypeBuilder<Notification_Image> builder)
        {

            builder.HasOne(no => no.notification).WithMany().HasForeignKey(or => or.NotiId);
            builder.HasOne(no => no.image).WithMany().HasForeignKey(no => no.ImageId);

            builder.ToTable("Notification_ImageItem");
        }
    }
}
