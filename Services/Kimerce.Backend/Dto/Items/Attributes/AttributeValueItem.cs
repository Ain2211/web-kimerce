using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Dto.Models;

namespace Kimerce.Backend.Dto.Items.Attributes
{
    public class AttributeValueItem: BaseModelInt
    {
        #region Fields

        /// <summary>
        /// Mã thuộc tính
        /// </summary>
        [MaxLength(512), Required]
        public string AttributeId { get; set; }

        /// <summary>
        /// Giá trị
        /// </summary>
        [Required]
        public string Value { get; set; }

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
