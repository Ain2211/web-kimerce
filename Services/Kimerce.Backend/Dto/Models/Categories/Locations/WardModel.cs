using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Domain.Locations;
using Kimerce.Backend.Dto.Models.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Categories
{
    public class WardModel : BaseCategoryModel
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

        /// <summary>
        /// ID cha
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Huyện
        /// </summary>
        public DistrictModel District { get; set; }

        /// <summary>
        /// Danh sách nhà kho
        /// </summary>
        public ICollection<WareHouseModel> WareHouses { get; set; }
        #endregion
    }
}
