using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.BaseEntities;

namespace Kimerce.Backend.Domain.Attributes
{
    public class Attribute : BaseEntityByInt
    {
        #region Constructors

        public Attribute()
        {
            Name = "";
            Description = "";
        }

        #endregion

        #region Fields

        /// <summary>
        /// Tên thuộc tính
        /// </summary>
        [MaxLength(512), Required]
        public string Name { get; set; }

        /// <summary>
        /// Mô tả thuộc tính
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
