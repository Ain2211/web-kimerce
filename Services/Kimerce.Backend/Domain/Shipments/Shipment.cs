using Kimerce.Backend.Domain.BaseEntities;
using Kimerce.Backend.Domain.Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.Shipments
{
    public class Shipment : BaseEntityByInt
    {
        #region Fields

        /// <summary>
        ///  Mã đơn hàng
        /// </summary>
        
        [Required]
       
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        /// <summary>
        /// Code tracking đơn hàng
        /// </summary>
        [MaxLength(512)]
        public string TrackingNumber { get; set; }
        /// <summary>
        /// Tổng trọng lượng
        /// </summary>
        public Decimal TotalWeight { get; set; }
        /// <summary>
        /// Trạng thái giao hàng
        /// </summary>
        public DeliveryStatus DeliveryStatus { get; set; }
        /// <summary>
        /// Ngày giao hàng thành công
        /// </summary>
        public DateTimeOffset CompleteDate { get; set; }
        /// <summary>
        /// Ngày bàn giao hàng hóa
        /// </summary>
        public DateTimeOffset HandoverDate { get; set; }
        /// <summary>
        /// Đơn hàng
        /// </summary>
       
        public ShipmentItem shipmentItem { get; set; }
        #endregion
    }
}
