using Kimerce.Backend.Domain.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Notification
{
    public class Notification_ImageModel
    {
        public int NotiId { get; set; }

        public  Domain.Notification.Notification notification { get; set; }

        public int ImageId { get; set; }

        public Image image { get; set; }
    }
}
