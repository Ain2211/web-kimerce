using Kimerce.Backend.Domain.Orders;
using Kimerce.Backend.Domain.Shipments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Shipments
{
    public class ShipmentModel : BaseModelInt
    {
        #region Fields

        /// <summary>
        ///  Mã đơn hàng
        /// </summary>
        [Required]
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
      
        #endregion
    }
}