using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.BaseEntities;
using Kimerce.Backend.Domain.Products;
using Kimerce.Backend.Dto.Models;

namespace Kimerce.Backend.Dto.Items.Products
{
    public class ProductCategoryItem: BaseModelInt
    {
        #region Constructors
        public ProductCategoryItem()
        {
            Status = StatusEnum.Editing;
            DefaultPageSize = 16;
            //PageSizeOptions = pageSizeOptions;
            //PriceRanges = priceRanges;
            ParentId = 0;
            ShortDescription = "";
            Description = "";
            IsActive = true;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Trạng thái danh mục
        /// </summary>
        public StatusEnum Status { get; set; }

        /// <summary>
        /// Giá trị phân trang mặc định
        /// </summary>
        public int DefaultPageSize { get; set; }

        /// <summary>
        /// Danh sách phân trang
        /// </summary>
        [MaxLength(256)]
        public string PageSizeOptions { get; set; }

        /// <summary>
        /// Phân khúc giá
        /// </summary>
        [MaxLength(512)]
        public string PriceRanges { get; set; }

        /// <summary>
        /// Mã danh mục cha
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Mô tả ngắn
        /// </summary>
        [MaxLength(1024)]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Mô tả đầy đủ (HTML)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Kích hoạt
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Tên danh mục
        /// </summary>
        [MaxLength(512)]
        public string Name { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        #endregion

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTimeOffset? CreatedTime { get; set; }

        public string CreatedTimeDisplay => CreatedTime.HasValue ? CreatedTime.Value.ToString("dd/MM/yyyy HH:mm") : "";

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public DateTimeOffset? UpdatedTime { get; set; }

        public string UpdatedTimeDisplay => UpdatedTime.HasValue ? UpdatedTime.Value.ToString("dd/MM/yyyy HH:mm") : "";
    }
}
