using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.News
{
    public class NewsImageModel: BaseModelInt
    {
        #region Fields
        /// <summary>
        /// Mã ảnh
        /// </summary>
        public int ImageId { get; set; }

        /// <summary>
        /// Mã Blog
        /// </summary>
        public int NewsId { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }

        #endregion
    }
}
