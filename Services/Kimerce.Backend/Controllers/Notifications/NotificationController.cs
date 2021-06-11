using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Notification;
using Kimerce.Backend.Dto.Models.Notification;
using Kimerce.Backend.Infrastructure.Services.Notifications;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Mvc;

namespace Kimerce.Backend.Controllers.Notifications
{
    [Route("[controller]")]
    [ApiController]

    public class NotificationController : ControllerBase
    {

        private readonly INotificationService _service;
        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Tìm kiếm thông báo
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var products = await _service.Search(param);
            return Ok(products);
        }



        /// <summary>
        /// Tạo / cập nhật thông báo
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>

        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]NotificationModel notification)
        {
            var result = await _service.CreateOrUpdate(notification);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông tin thông báo
        /// </summary>
        /// <param name="id">Id của thông báo</param>
        /// <returns>Product object</returns>
        [HttpGet("GetById/{id}")]
        public Notification Get(int id)
        {
            return _service.GetId(id);
        }


        /// <summary>
        /// Xóa thông báo
        /// </summary>
        /// <param name="id">Mã thông báo cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }

    }
}