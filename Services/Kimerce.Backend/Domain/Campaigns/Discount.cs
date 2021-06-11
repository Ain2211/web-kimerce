using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.BaseEntities;

namespace Kimerce.Backend.Domain.Campaigns
{
    public class Discount : BaseEntityByInt
    {
        #region Constructors
        public Discount()
        {
            DateTimeOffset currentTime = new DateTimeOffset();
            currentTime = DateTime.Now.ToLocalTime();
            Name = "";
            UsePercentage = true;
            DiscountPercentage = 0;
            DiscountAmount = 0;
            MaxDiscountAmount = 0;
            StartDate = currentTime;
            EndDate = currentTime;
            RequireCouponCode = true;
            CouponCode = "";
            Quantity = 0;
            DiscountOrders = new List<DiscountOrder>();
        }

        #endregion

        #region Fields
        /// <summary>
        /// Tên mã giảm giá
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
        /// <summary>
        /// Số lượng mã
        /// </summary>
        public int Quantity { get; set; }

        public virtual ICollection<DiscountOrder> DiscountOrders { get; set; }
        #endregion

        #region Methods

        #endregion

    }

}
