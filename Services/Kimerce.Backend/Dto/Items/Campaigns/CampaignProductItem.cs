using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Campaigns;
using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Dto.Models;

namespace Kimerce.Backend.Dto.Items.Campaigns
{
    public class CampaignProductItem : BaseModelInt
    {
        #region Fields
        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Mã chiến dịch
        /// </summary>
        public int CampaignId { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }
        public CampaignItem CampaignItem { get; set; }
        #endregion


    }
}
