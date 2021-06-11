using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.BaseEntities;

namespace Kimerce.Backend.Domain.Campaigns
{
    public class CampaignOrder : BaseEntityByInt
    {
        #region Constructors
        public CampaignOrder()
        {
            OrderId = 0;
            CampaignId = 0;
            DisplayOrder = 1;
            Campaign = new Campaign();

        }

        #endregion

        #region Fields
        /// <summary>
        /// Mã đơn hàng
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Mã chiến dịch
        /// </summary>
        public int CampaignId { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }
        public Campaign Campaign {get;set;}

        #endregion

        #region Methods

        #endregion

    }
}
