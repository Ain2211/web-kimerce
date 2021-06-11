using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Dto.Models;

namespace Kimerce.Backend.Dto.Items.Products
{
    public class RelateProductItem : BaseModelInt
    {
        #region Fields
        /// <summary>
        /// Mã A
        /// </summary>
        [Required]
        public int ProductIdA { get; set; }

        /// <summary>
        /// Mã B
        /// </summary>
        public int ProductIdB { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; }

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
