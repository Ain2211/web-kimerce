using Kimerce.Backend.Dto.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Items.Tracking
{
    public class TrackingItem : BaseModelInt
    {

        #region Constructors
        public TrackingItem()
        {
            Title = "";
            SubTitle = "";
        }
        #endregion

        #region Fields
        /// <summary>
        /// Tên lịch sử
        /// </summary>
        [Required]
        public String Title { set; get; } 

        /// <summary>
        /// Tên lịch sử rút gọn
        /// </summary>
        [Required]
        public String SubTitle { set; get; } 

        /// <summary>
        /// Id thực thể 
        /// </summary>
        public int EntityId { set; get; }

        /// <summary>
        /// Kiểu thực thể 
        /// </summary>
        public EntityType EntityType { get; set; }

        /// <summary>
        /// Kiểu hành động 
        /// </summary>
        public ActionType ActionType { get; set; }

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
        Email = 4
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
        ViewDetail = 4


    }

    #endregion

}

