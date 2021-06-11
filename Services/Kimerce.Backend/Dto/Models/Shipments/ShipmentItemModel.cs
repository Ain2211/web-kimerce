using Kimerce.Backend.Domain.Shipments;
using Kimerce.Backend.Domain.WareHouse;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Shipments
{
    public class ShipmentItemModel : BaseModelInt
    {
        /// <summary>
        /// Mã đơn vận chuyển
        /// </summary>
        [Required]
        public int ShipmentId { get; set; }
        /// <summary>
        /// Mã item của đơn hàng
        /// </summary>
        [MaxLength(512)]
        public string OrderItemId { get; set; }
        /// <summary>
        /// Số lượng
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Mã kho hàng
        /// </summary>
        public int WareHouseId { get; set; }
      
     
    }
}