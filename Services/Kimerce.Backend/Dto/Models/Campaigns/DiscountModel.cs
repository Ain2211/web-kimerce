using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.BaseEntities;
using Kimerce.Backend.Dto.Models;

namespace Kimerce.Backend.Dto.Models.Campaigns
{
    public class DiscountModel : BaseModelInt
    {
        #region Fields
        /// <summary>
        /// Tên chiến dịch
        /// </summary>
        [MaxLength(512)]
        [Required]
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
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Ngày két thúc
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// Yêu cầu mã giảm giá
        /// </summary>
        public bool RequireCouponCode { get; set; }

        /// <summary>
        /// Mã giảm giá
        /// </summary>
        [MaxLength(128)]
        public String CouponCode { get; set; }
        public int Quantity { get; set; }

        public ICollection<DiscountOrderModel> DiscountOrderModels { get; set; }
        #endregion


    }

}
