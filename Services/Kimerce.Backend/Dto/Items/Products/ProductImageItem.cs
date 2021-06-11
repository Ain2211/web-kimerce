using Kimerce.Backend.Domain.Products;
using Kimerce.Backend.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Products
{
    public class ProductImageItem : BaseModelInt
    {
        #region Fields
        /// <summary>
        /// Mã ảnh
        /// </summary>
        public int ImageId { get; set; }

        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }

        #endregion
    }
}
