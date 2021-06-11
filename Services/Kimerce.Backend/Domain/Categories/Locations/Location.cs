using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.Categories
{
    public class Location : BaseCategory
    {

        #region Fields
        /// <summary>
        /// Mã hành chính
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string Code { get; set; }

        /// <summary>
        /// Tên không dấu
        /// </summary>
        [MaxLength(256)]
        public string UnsignName { get; set; }

        /// <summary>
        /// Miền
        /// </summary>
        [Required]
        public CityRealm CityRealm { get; set; }
        #endregion
    }

    public enum CityRealm
    {
        [Description("Miền Bắc")]
        Northern = 1,
        [Description("Miền Trung")]
        Central = 2,
        [Description("Miền Nam")]
        South = 3,
        [Description("Hà Nội")]
        HN = 4,
        [Description("Hồ Chí Minh")]
        HCM = 5
    }
}
