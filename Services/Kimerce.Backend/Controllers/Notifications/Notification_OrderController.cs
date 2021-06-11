using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Notification;
using Kimerce.Backend.Dto.Items.Orders;
using Kimerce.Backend.Dto.Models.Notification;
using Kimerce.Backend.Infrastructure.Services.Notifications;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Mvc;

namespace Kimerce.Backend.Controllers.Notifications
{

    [Route("[controller]")]
    [ApiController]
    public class Notification_OrderController : ControllerBase
    {
        private readonly INotification_OrderService notification_OrderService;

        public Notification_OrderController(INotification_OrderService notification_OrderService)
        {
            this.notification_OrderService = notification_OrderService;
        }

        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var orders = await notification_OrderService.Search(param);
            return Ok(orders);
        }


        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]Notification_OrderModel order)
        {
            var result = await notification_OrderService.CreateOrUpdate(order);
            return Ok(result);
        }


        [HttpGet("GetById/{id}")]
        public Notification_Order Get(int id)
        {
            return notification_OrderService.Get(id);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await notification_OrderService.Delete(id);
            return Ok(result);
        }

        [HttpGet("GetNotificationByOrderId/{id}")]
        public IQueryable<Notification> GetNotificationById(int id)
        {
            return notification_OrderService.GgetNotificationByOrder(id);
        }

        [HttpGet("GetOrderByNotificationId/{id}")]
        public IQueryable<OrderItem> GetOrdersById(int id)
        {
            return notification_OrderService.GetOrdersByNotificationId(id);
        }



    }
}