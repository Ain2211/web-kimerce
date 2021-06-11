using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Dto.Models;

namespace Kimerce.Backend.Dto.Items.Campaigns
{
    public class DiscountItem : BaseModelInt
    {
        #region Fields
        /// <summary>
        /// Tên mã giảm giá
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Sử dụng % hay không
        /// </summary>
        public bool UsePercentage { get; set; }

        /// <summary>
        /// % giảm giá
        /// </summary>
        public int DiscountPercentage { get; set; }

        /// <summary>
        /// Số lượng giảm giá
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Giá trị giảm tối đa
        /// </summary>
        public decimal MaxDiscountAmount { get; set; }

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
        /// Yêu cầu mã giảm giá
        /// </summary>
        public bool RequireCouponCode { get; set; }

        /// <summary>
        /// Mã giảm giá
        /// </summary>
        public String CouponCode { get; set; }
        /// <summary>
        /// Số lượng mã
        /// </summary>
        public int Quantity { get; set; }

        public ICollection<DiscountOrderItem> DiscountOrderItems { get; set; }

        #endregion
    }
}
