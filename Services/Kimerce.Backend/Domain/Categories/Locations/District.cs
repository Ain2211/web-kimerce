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
    public class District : Location
    {
        #region Constructors
        public District()
        {
            Name = "";
            DisplayOrder = 0;
            Code = "";
            UnsignName = "";
            CityRealm = 0;
            ParentId = 0;
            City = new City();
            Wards = new List<Ward>();
        }

        #endregion

        #region Fields
        /// <summary>
        /// ID cha
        /// </summary>
        [Required]
        public int ParentId { get; set; }

        /// <summary>
        /// Tỉnh
        /// </summary>
        public City City { get; set; }


        /// <summary>
        /// Danh sách xã
        /// </summary>
        public ICollection<Ward> Wards { get; set; }
        #endregion
    }

}
