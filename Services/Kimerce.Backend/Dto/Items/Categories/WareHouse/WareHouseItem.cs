using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Dto.Items.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Categories.WareHouse
{
    public class WareHouseItem : BaseCategory
    {
        #region Fields
        /// <summary>
        /// Id Xã
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Xã
        /// </summary>
        public WardItem Ward { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Vĩ độ
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Kinh Độ
        /// </summary>
        public double Longtitude { get; set; }

        /// <summary>
        /// Số điện thoại người liên hệ
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Tên người liên hệ
        /// </summary>
        public string ContactName { get; set; }
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
