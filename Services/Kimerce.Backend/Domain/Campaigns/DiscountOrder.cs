using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.BaseEntities;

namespace Kimerce.Backend.Domain.Campaigns
{
    public class DiscountOrder : BaseEntityByInt
    {
        #region Constructors
        public DiscountOrder()
        {
            OrderId = 0;
            DiscountId = 0;
            Discount = new Discount();
        }

        #endregion

        #region Fields
        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Mã giảm giá
        /// </summary>
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }

        #endregion

        #region Methods

        #endregion

    }
}
