using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.BaseEntities;

namespace Kimerce.Backend.Domain.Products
{
    public class RelateProduct: BaseEntityInt
    {
        #region Constructors
        public RelateProduct()
        {

        }

        #endregion

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

        /// <summary>
        /// Sản phẩm A
        /// </summary>
        public Product ProductA { get; set; }

        /// <summary>
        /// Sản phẩn B
        /// </summary>
        public Product ProductB { get; set; }
        #endregion

        #region Methods

        #endregion
    }
}
