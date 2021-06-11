using Kimerce.Backend.Domain.Products;
using Kimerce.Backend.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Products
{
    public class ProductItem : BaseModelInt
    {

        #region Fields
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mô tả ngắn
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Mô tả đầy đủ (HTML)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Mã SKU
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// Giá bán
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Giá cũ
        /// </summary>
        public decimal OldPrice { get; set; }

        /// <summary>
        /// Gọi để biết giá
        /// </summary>
        public bool CallForPrice { get; set; }

        /// <summary>
        /// Trạng thái sản phẩm
        /// </summary>
        public StatusEnum Status { get; set; }

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
