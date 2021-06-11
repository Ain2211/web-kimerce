using Kimerce.Backend.Domain.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Dto.Models.Notification
{
    public class NotificationModel : BaseModelLong
    {
        #region Constructors
        public NotificationModel()
        {
            Title = "";
            SubTitle = "";
            Link = "";
            UserId = 0;
            TypeId = 0;
            IsRead = false;
            ReadDate = DateTimeOffset.Now;
        }

        #endregion

        #region Fields
        /// <summary>
        /// Tên thông báo
        /// </summary>
        [MaxLength(512)]
        [Required]
        public String Title { set; get; }

        /// <summary> 
        /// Tên thông báo rút gọn
        /// </summary>
        [MaxLength(512)]
        public String SubTitle { set; get; }

        /// <summary>
        /// Tên đường link 
        /// </summary>
        [MaxLength(1024)]
        [Required]
        public String Link { set; get; }
        /// <summary>
        /// Id người dùng
        /// </summary>
        [Required]
        public int UserId { set; get; }

        /// <summary>
        /// Id loại 
        /// </summary>
        [Required]
        public int TypeId { set; get; }
        /// <summary>
        /// Đã đọc chưa
        /// </summary>
        public bool IsRead { set; get; }
        /// <summary>
        /// Thời gian đọc 
        /// </summary>
        public DateTimeOffset ReadDate { get; set; }

        public List<Notification_Order> notification_Orders { get; set; }
        #endregion
    }
}
