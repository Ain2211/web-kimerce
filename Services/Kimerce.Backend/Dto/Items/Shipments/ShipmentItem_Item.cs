using Kimerce.Backend.Domain.Shipments;
using Kimerce.Backend.Domain.WareHouse;
using Kimerce.Backend.Dto.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Shipments
{
    public class ShipmentItem_Item : BaseModelInt
    {
        #region Fields
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
