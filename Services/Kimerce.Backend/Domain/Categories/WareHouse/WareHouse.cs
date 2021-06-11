using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Domain.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.WareHouse
{
    public class WareHouse : BaseCategory
    {

        #region Constructors
        public WareHouse()
        {
            Name = "";
            DisplayOrder = 0;
            Address = "";
            Latitude = 0;
            Longtitude = 0;
            PhoneNumber = "";
            ContactName = "";
            ParentId = 0;
            Ward = new Ward();
        }

        #endregion

        #region Fields
        /// <summary>
        /// Id Xã
        /// </summary>
        [Required]
        public int ParentId { get; set; }

        /// <summary>
        /// Xã
        /// </summary>
        public Ward Ward { get; set; }

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
        /// Kinh Độ
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
