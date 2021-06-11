using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Campaigns;
using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Dto.Models;

namespace Kimerce.Backend.Dto.Items.Campaigns
{
    public class CampaignItem : BaseModelInt
    {
        #region Fields
        /// <summary>
        /// Tên chiến dịch
        /// </summary>
        public String Subject { get; set; }

        /// <summary>
        /// Mô tả chiến dịch
        /// </summary>
        public String Body { get; set; }

        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }
        public string StartDateDisplay => StartDate.HasValue ? StartDate.Value.ToString("dd/MM/yyyy HH:mm") : "";

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        public string EndDateDisplay => EndDate.HasValue ? EndDate.Value.ToString("dd/MM/yyyy HH:mm") : "";

        /// <summary>
        /// Ngân sách
        /// </summary>
        public decimal Budget { get; set; }

        /// <summary>
        /// Tiến độ
        /// </summary>
        public CampaignStatus Status { get; set; }

        public ICollection<CampaignOrderItem> CampaignOrderItems { get; set; }
        
        public ICollection<CampaignProductItem> GetCampaignProductItems { get; set; }

        #endregion
    }
}
