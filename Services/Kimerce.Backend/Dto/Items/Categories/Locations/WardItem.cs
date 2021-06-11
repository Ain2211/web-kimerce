using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Domain.Locations;
using Kimerce.Backend.Domain.WareHouse;
using Kimerce.Backend.Dto.Items.Categories.WareHouse;
using Kimerce.Backend.Dto.Models.Products;
using Kimerce.Backend.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Locations
{
    public class WardItem : BaseCategoryModel
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
        /// ID cha
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Huyện
        /// </summary>
        public DistrictItem District { get; set; }

        /// <summary>
        /// Danh sách nhà kho
        /// </summary>
        public ICollection<WareHouseItem> WareHouses { get; set; }
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
