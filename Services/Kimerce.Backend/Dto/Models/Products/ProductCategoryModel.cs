using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Products;

namespace Kimerce.Backend.Dto.Models.Products
{
    public class ProductCategoryModel: BaseModelInt
    {
        #region Constructors
        public ProductCategoryModel()
        {
            Status = StatusEnum.Editing;
            DefaultPageSize = 16;
            //PageSizeOptions = pageSizeOptions;
            //PriceRanges = priceRanges;
            ParentId = null;
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
    }
}
