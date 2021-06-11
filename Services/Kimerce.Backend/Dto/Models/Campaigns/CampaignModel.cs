using Kimerce.Backend.Domain.Campaigns;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Campaigns
{
    public class CampaignModel : BaseModelInt
    {
        #region Fields
        /// <summary>
        /// Tên chiến dịch
        /// </summary>
        [Required(ErrorMessage = "Tên sản phẩm không để trống")]
        public string Subject { get; set; }

        /// <summary>
        /// Mô tả ngắn
        /// </summary>
        [MaxLength(512)]
        public string Body { get; set; }
        /// <summary>
        /// Budget
        /// </summary>
        public decimal Budget { get; set; }

        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// Trạng thái 
        /// </summary>
        public CampaignStatus Status { get; set; }

        public ICollection<CampaignOrderModel> CampaignOrderModels { get; set; }
        public ICollection<CampaignProductModel> CampaignProductModels { get; set; }

        #endregion
    }
}
