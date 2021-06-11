using Kimerce.Backend.Dto.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Categories
{
    public class WareHouseModel : BaseCategoryModel
    {
        #region Fields
        /// <summary>
        /// Id Xã
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Xã
        /// </summary>
        public WardModel Ward { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [MaxLength(1024)]
        public string Address { get; set; }

        /// <summary>
        /// Vĩ độ
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Kinh độ
        /// </summary>
        public double Longtitude { get; set; }

        /// <summary>
        /// Số điện thoại người liên hệ
        /// </summary>
        [MaxLength(256)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Tên người liên hệ
        /// </summary>
        [MaxLength(256)]
        public string ContactName { get; set; }
        #endregion
    }
}
