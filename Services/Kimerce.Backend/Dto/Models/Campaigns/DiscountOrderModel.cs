using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Campaigns;
using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Dto.Models;

namespace Kimerce.Backend.Dto.Models.Campaigns
{
    public class DiscountOrderModel : BaseModelInt
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
        public DiscountModel DiscountModel { get; set; }

        #endregion


    }
}
