using Kimerce.Backend.Domain.BaseEntities;
using Kimerce.Backend.Domain.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.Notification
{
    public class Notification_Image : BaseEntityByInt
    {
        public int ImageId { get; set; }

        public Image image { get; set; }

        public int NotiId { set; get; }

        public Notification notification { set; get; }


    }
}
