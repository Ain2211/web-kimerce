using Kimerce.Backend.Domain.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace Kimerce.Backend.Domain.Categories
{
    public class BaseCategory : BaseEntityByInt
    {
        #region Fields
        /// <summary>
        /// Tên
        /// </summary>
        [MaxLength(512)]
        public string Name { get; set; }


        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; } 

        #endregion
    }
}
