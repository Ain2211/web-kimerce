using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.BaseEntities;
using Kimerce.Backend.Domain.Images;

namespace Kimerce.Backend.Domain.Products
{
    public class Product : BaseEntityByInt
    {
        #region Constructors
        public Product()
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
        [MaxLength(512)]
        [Required]
        public string Name { get; set; }

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
        /// Mã SKU
        /// </summary>
        [MaxLength(512)]
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

        /// <summary>
        /// Các sản phẩm liên quan
        /// </summary>
        public ICollection<RelateProduct> RelateChildren { get; set; }

        /// <summary>
        /// Chưa nghĩ ra chú thích .-.
        /// </summary>
        public ICollection<RelateProduct> RelateParent { get; set; }

        //public ICollection<Image> Image { get; set; }
        
        #endregion

        #region Methods

        #endregion

    }

    public enum StatusEnum
    {
        [Description("Đang biên tập")]
        Editing = 1,
        [Description("Đang chờ duyệt")]
        Pending = 2,
        [Description("Đã duyệt")]
        Approved = 3,
        [Description("Đã xuất bản")]
        Published = 4
    }
}
