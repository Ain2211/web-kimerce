using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Images;

namespace Kimerce.Backend.Domain.News
{
    public class NewsImage:BaseEntities.BaseEntityInt
    {
        #region Fields
        /// <summary>
        /// Mã ảnh
        /// </summary>
        public int ImageId { get; set; }

        /// <summary>
        /// Mã blog
        /// </summary>
        public int NewsId { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Blog
        /// </summary>
        public virtual News News { get; set; }

        /// <summary>
        /// Hình ảnh
        /// </summary>
        public virtual Image Image { get; set;}
        #endregion
    }
}
