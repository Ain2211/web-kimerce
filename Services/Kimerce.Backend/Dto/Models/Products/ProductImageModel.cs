using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Products
{
    public class ProductImageModel: BaseModelInt
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
