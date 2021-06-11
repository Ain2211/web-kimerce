using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Products
{
    public class BaseCategoryModel : BaseModelInt
    {
        #region Fields
        /// <summary>
        /// Tên 
        /// </summary>
        [Required(ErrorMessage = "Tên không để trống")]
        public string Name { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        ///// <summary>
        ///// Đã bị xóa hay chưa?
        ///// </summary>
        public bool IsDeleted { get; set; }

        ///// <summary>
        ///// Id của người tạo
        ///// </summary>
        public int? CreatedBy { get; set; }

        ///// <summary>
        ///// Tên của người tạo
        ///// </summary>
        [MaxLength(150)]
        public string CreatedByUserName { get; set; }

        ///// <summary>
        ///// Id của người sửa
        ///// </summary>
        public int? UpdatedBy { get; set; }

        ///// <summary>
        ///// Tên của người sửa
        ///// </summary>
        [MaxLength(150)]
        public string UpdatedByUserName { get; set; }
        #endregion
    }
}