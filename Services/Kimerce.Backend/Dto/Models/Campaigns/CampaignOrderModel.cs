using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Campaigns;
using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Dto.Models;

namespace Kimerce.Backend.Dto.Models.Campaigns
{
    public class CampaignOrderModel : BaseModelInt
    {
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

        public CampaignModel CampaignModel { get; set; }

        #endregion


    }
}
