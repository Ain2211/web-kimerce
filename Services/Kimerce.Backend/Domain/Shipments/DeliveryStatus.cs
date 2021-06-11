using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.Shipments
{
    public enum DeliveryStatus
    {
        [Description("Chờ bàn giao")]
        WaitToHandOver = 1,
        [Description("Đã Bàn Giao")]
        Shipped = 2,
        [Description("Đang Vận Chuyển")]
        Transit = 3,
        [Description("Đang giao hàng")]
        Delivery = 4,
        [Description("Đang trả về")]
        Return = 5,
        [Description("Đã giao hàng")]
        Delivered = 6,
        [Description("Đã trả về")]
        Returned = 7
    }
}
