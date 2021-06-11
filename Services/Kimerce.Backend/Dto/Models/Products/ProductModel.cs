using Kimerce.Backend.Domain.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Products
{
    public class ProductModel : BaseModelInt
    {
        #region Constructors
        public ProductModel()
        {
            Name = "";
            ShortDescription = "";
            Description = "";
            Sku = "";
            Price = 0;
            OldPrice = 0;
            CallForPrice = false;
            Status = StatusEnum.Editing;
        }

        #endregion

        #region Fields
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        [Required(ErrorMessage = "Tên sản phẩm không để trống")]
        public string Name { get; set; }

        /// <summary>
        /// Mô tả ngắn
        /// </summary>
        [MaxLength(512)]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Mô tả đầy đủ (HTML)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Mã SKU
        /// </summary>
        [MaxLength(512)]
        public string Sku { get; set; }

        /// <summary>
        /// Giá bán
        /// </summary>
        public decimal Price { get; set; } = 0;

        /// <summary>
        /// Giá cũ
        /// </summary>
        public decimal OldPrice { get; set; } = 0;

        /// <summary>
        /// Gọi để biết giá
        /// </summary>
        public bool CallForPrice { get; set; } = false;

        /// <summary>
        /// Trạng thái sản phẩm
        /// </summary>
        public StatusEnum Status { get; set; } = StatusEnum.Editing;

        #endregion


    }
}
