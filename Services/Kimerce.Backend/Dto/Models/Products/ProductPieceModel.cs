using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Products
{
    public class ProductPieceModel: BaseModelLong
    {
        #region Fields
        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// Mã thuộc tính tốn kho
        /// </summary>
        [Required]
        public int InventoryAttributeId { get; set; }

        /// <summary>
        /// Mã giá trị thuộc tính
        /// </summary>
        [Required]
        public int AttributeValueId { get; set; }

        /// <summary>
        /// Sỗ lượng
        /// </summary>
        [Required]
        public int Quantity { get; set; }
        #endregion Fields
    }
}
