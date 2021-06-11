using Kimerce.Backend.Domain.BaseEntities;
using Kimerce.Backend.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.Notification
{
    public class Notification_Order : BaseEntityByInt
    {
        public int OrderId { get; set; }

        public Order order { get; set; }
        public int NotiId { get; set; }

        public Notification notification { get; set; }


    }
}
