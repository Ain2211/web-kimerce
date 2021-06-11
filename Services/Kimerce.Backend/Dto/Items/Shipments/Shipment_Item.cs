using Kimerce.Backend.Domain.Orders;
using Kimerce.Backend.Domain.Shipments;
using Kimerce.Backend.Dto.Models;
using Kimerce.Backend.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Shipments
{
    public class Shipment_Item : BaseModelInt
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
        public string DeliveryStatusStr => DeliveryStatus.GetDescription();
        /// <summary>
        /// Ngày giao hàng thành công
        /// </summary>
        public DateTimeOffset? CompleteDate { get; set; }
        public string CompleteDateDisplay => CompleteDate.HasValue ? CompleteDate.Value.ToString("dd/MM/yyyy HH:mm") : "";
        /// <summary>
        /// Ngày bàn giao hàng hóa
        /// </summary>
        public DateTimeOffset? HandoverDate { get; set; }
        public string HandoverDateDisplay => HandoverDate.HasValue ? HandoverDate.Value.ToString("dd/MM/yyyy HH:mm") : "";

        #endregion
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTimeOffset? CreatedTime { get; set; }

        public string CreatedTimeDisplay => CreatedTime.HasValue ? CreatedTime.Value.ToString("dd/MM/yyyy HH:mm") : "";

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public DateTimeOffset? UpdatedTime { get; set; }

        public string UpdatedTimeDisplay => UpdatedTime.HasValue ? UpdatedTime.Value.ToString("dd/MM/yyyy HH:mm") : "";
    }
}
