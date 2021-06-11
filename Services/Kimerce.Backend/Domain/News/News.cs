using Kimerce.Backend.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.News
{
    public class News : BaseEntityByInt
    {
        #region Constructors
        public News()
        {
            Title = "";
            ShortDescription = "";
            Description = "";
            Status = NewsStatus.Editing;
        }

        #endregion

        #region Fields
        /// <summary>
        /// Tiêu đề
        /// </summary>
        [MaxLength(512)]
        [Required]
        public string Title { get; set; } = "";
        /// <summary>
        /// Mô tả ngắn
        /// </summary>
        [MaxLength(1024)]
        public string ShortDescription { get; set; } = "";
        /// <summary>
        /// Nội dung
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Trạng thái blog
        /// </summary>
        public NewsStatus Status { get; set; }
        /// <summary>
        /// Ngày công bố
        /// </summary>
        public DateTimeOffset PublishDate { get; set; }
        /// <summary>
        /// Ngày bắt đầu có hiệu lực
        /// </summary>
        public DateTimeOffset StartDate { get; set; }
        /// <summary>
        /// Ngày hết hiệu lực
        /// </summary>
        public DateTimeOffset EndDate { get; set; }
        #endregion

        #region Methods

        #endregion

    }

    public enum NewsStatus
    {
        [Description("Sản phẩm")]
        Editing = 1,
        [Description("Người dùng")]
        Pending = 2,
        [Description("Danh mục")]
        Published = 3
    }
}
