using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Products
{
    public class RelateProductModel : BaseModelInt
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

    }
}
