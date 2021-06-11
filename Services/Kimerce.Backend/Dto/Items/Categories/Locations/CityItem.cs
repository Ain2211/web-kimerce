using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Domain.Locations;
using Kimerce.Backend.Dto.Items.Locations;
using Kimerce.Backend.Dto.Models;
using Kimerce.Backend.Dto.Models.Products;
using Kimerce.Backend.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Categories
{
    public class CityItem : BaseCategoryModel
    {
        #region Fields
        /// <summary>
        /// Mã hành chính
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên không dấu
        /// </summary>
        public string UnsignName { get; set; }

        /// <summary>
        /// Miền
        /// </summary>
        public CityRealm CityRealm { get; set; }

        public string CityRealmStr => CityRealm.GetDescription();

        /// <summary>
        /// Danh sách huyện
        /// </summary>
        public ICollection<DistrictItem> Districts { get; set; }
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
