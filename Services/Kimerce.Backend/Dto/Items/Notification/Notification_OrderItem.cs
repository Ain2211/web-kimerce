using Kimerce.Backend.Domain.Orders;
using Kimerce.Backend.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Notification
{
    public class Notification_OrderItem : BaseModelInt
    {
        public int OrderId { get; set; }

        public Domain.Notification.Notification notification { get; set; }

        public int NotiId { get; set; }

        public Order order { get; set; }




    }
}
