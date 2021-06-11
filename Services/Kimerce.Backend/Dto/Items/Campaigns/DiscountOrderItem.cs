using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Dto.Models;

namespace Kimerce.Backend.Dto.Items.Campaigns
{
    public class DiscountOrderItem : BaseModelInt
    {
        #region Fields
        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Mã giảm giá
        /// </summary>
        public int DiscountId { get; set; }
        public DiscountItem DiscountItem { get; set; }

        #endregion
    }
}
