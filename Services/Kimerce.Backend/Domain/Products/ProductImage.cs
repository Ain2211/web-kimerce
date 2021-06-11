using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Images;

namespace Kimerce.Backend.Domain.Products
{
    /// <summary>
    /// Liên kết ản-sản phẩm
    /// </summary>
    public class ProductImage:BaseEntities.BaseEntityInt
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

        /// <summary>
        /// Sản phẩm
        /// </summary>
        public virtual Product Product { get; set; }
        
        /// <summary>
        /// Hình ảnh
        /// </summary>
        public virtual Image Image { get; set; }
        #endregion
    }
}
