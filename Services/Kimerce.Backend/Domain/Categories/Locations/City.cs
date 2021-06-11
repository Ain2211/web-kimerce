using Kimerce.Backend.Domain.Locations;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kimerce.Backend.Domain.Categories
{
    public class City : Location
    {
        #region Constructors
        public City()
        {
            Name = "";
            DisplayOrder = 0;
            Code = "";
            UnsignName = "";
            CityRealm = 0;
            Districts = new List<District>();
        }

        #endregion

        #region Fields
        /// <summary>
        /// Danh sách huyện
        /// </summary>
        public ICollection<District> Districts { get; set; }
        #endregion
    }
}
