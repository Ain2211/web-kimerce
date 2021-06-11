using Kimerce.Backend.Domain.Categories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.Locations
{
    public class Ward : Location
    {
        #region Constructors
        public Ward()
        {
            Name = "";
            DisplayOrder = 0;
            Code = "";
            UnsignName = "";
            CityRealm = 0;
            ParentId = 0;
            District = new District();
            WareHouses = new List<WareHouse.WareHouse>();
        }

        #endregion

        #region Fields
        /// <summary>
        /// ID cha
        /// </summary>
        [Required]
        public int ParentId { get; set; }

        /// <summary>
        /// Huyện
        /// </summary>
        public District District { get; set; }

        /// <summary>
        /// Danh sách nhà kho
        /// </summary>
        public ICollection<WareHouse.WareHouse> WareHouses { get; set; }

        #endregion
    }

}
