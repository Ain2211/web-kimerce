using Kimerce.Backend.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Domain.Tracking
{
    public class Tracking : BaseEntityByInt
    {


        #region Constructors
        public Tracking()
        {
            Title = "";
            SubTitle = "";
            EntityId = 1;
            
        }

        #endregion

        #region Fields
        /// <summary>
        /// Tên lịch sử
        /// </summary>
        [MaxLength(512)]
        [Required(ErrorMessage = "Không để trống tên")]


       
        public String Title { set; get; }

        /// <summary>
        /// Tên lịch sử gọn
        /// </summary>
        [MaxLength(1024)]
        public String SubTitle { set; get; }

        /// <summary>
        /// Id thực thể 
        /// </summary>

        public int EntityId { set; get; }

        /// <summary>
        /// Loại thực thể 
        /// </summary>

        public EntityType EnType { get; set; }

        /// <summary>
        /// Kiểu hành động
        /// </summary>

        public ActionType AcType { get; set; }


       
    }

    public enum EntityType
    {
        [Description("Sản phẩm")]
        Product = 1,
        [Description("Người dùng")]
        User = 2,
        [Description("Danh mục")]
        Category = 3,
        [Description("Email")]
        Email = 4,
    }

    public enum ActionType
    {

        [Description("Thêm")]
        Add = 1,
        [Description("Cập nhật")]
        Update = 2,
        [Description("Xóa")]
        Delete = 3,
        [Description("Xem chi tiết")]
        ViewDetail = 4,


    }

    #endregion
}
