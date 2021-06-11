using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.BaseEntities;

namespace Kimerce.Backend.Domain.Campaigns
{
    public class Campaign : BaseEntityByInt
    {
        #region Constructors
        public Campaign()
        {
            DateTimeOffset currentTime = new DateTimeOffset();
            currentTime = DateTime.Now.ToLocalTime();
            Subject = "";
            Body = "";
            StartDate = currentTime;
            EndDate = currentTime;
            Budget = 0;
            Status = CampaignStatus.Pending;
            CampaignOrders = new List<CampaignOrder>();
            CampaignProducts = new List<CampaignProduct>();
        }

        #endregion

        #region Fields
        /// <summary>
        /// Tên chiến dịch
        /// </summary>
        [MaxLength(512)]
        [Required]
        public String Subject { get; set; }

        /// <summary>
        /// Mô tả chiến dịch
        /// </summary>
        public String Body { get; set; }

        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Ngày két thúc
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// Ngân sách
        /// </summary>
        public decimal Budget { get; set; }

        /// <summary>
        /// Tiến độ
        /// </summary>
        public CampaignStatus Status { get; set; }

        public virtual ICollection<CampaignOrder> CampaignOrders { get; set; }

        public virtual ICollection<CampaignProduct> CampaignProducts { get; set; }
        #endregion

        #region Methods

        #endregion

    }

    public enum CampaignStatus
    {
        [Description("Đang chờ")]
        Pending = 1,
        [Description("Đang chạy")]
        Active = 2,
        [Description("Đã hết hạn")]
        Expired = 3,
        [Description("Không kích hoạt")]
        InActive = 4
    }
}
