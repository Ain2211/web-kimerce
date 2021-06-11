using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Attributes
{
    public class AttributeValueModel: BaseModelInt
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
    }
}
