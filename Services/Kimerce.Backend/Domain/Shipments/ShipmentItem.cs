using Kimerce.Backend.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.Shipments
{
    public class ShipmentItem : BaseEntityByInt
    {
        /// <summary>
        /// Mã đơn vận chuyển
        /// </summary>
        
        [Required]
        
        [ForeignKey("Shipment")]
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
        public Shipment shipment { get; set; }
       
       
        public int WareHouseId { get; set; }
       
    }
}
