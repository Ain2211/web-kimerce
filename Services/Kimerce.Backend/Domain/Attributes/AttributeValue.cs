using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.BaseEntities;

namespace Kimerce.Backend.Domain.Attributes
{
    public class AttributeValue: BaseEntityByInt
    {
        #region Constructors

        public AttributeValue()
        {
            AttributeId = "";
            Value = "";
        }

        #endregion

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
