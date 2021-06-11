using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Categories;

namespace Kimerce.Backend.Domain.Products
{
    public class ProductCategory: BaseCategory
    {
        #region Constructors
        public ProductCategory()
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
        /// Danh mục cha
        /// </summary>
        public virtual ProductCategory Parent { get; set; }

        /// <summary>
        /// Danh mục con
        /// </summary>
        public virtual ICollection<ProductCategory> Children { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
