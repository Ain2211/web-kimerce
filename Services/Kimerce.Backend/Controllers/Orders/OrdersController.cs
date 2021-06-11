using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kimerce.Backend.Domain.Orders;
using Kimerce.Backend.Infrastructure.Data;
using Kimerce.Backend.Infrastructure.Services.Orders;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.SmartTable;
using Kimerce.Backend.Dto.Models.Orders;
using Kimerce.Backend.Dto;

namespace Kimerce.Backend.Controllers.Orders
{
    [Route("[controller]")]

    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            this._orderService = orderService;
        }

        //[HttpGet("GetAll")]

        //public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        //{
        //    return await _orderService.Index();
        //}


        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var orders = await _orderService.Search(param);
            return Ok(orders);
        }


        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]OrderModel order)
        {
            var result = await _orderService.CreateOrUpdate(order);
            return Ok(result);
        }


        [HttpGet("GetById/{id}")]
        public Dto.Items.Orders.OrderItem Get(int id)
        {
            return _orderService.Get(id).ToItem();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.Delete(id);
            return Ok(result);
        }
    }
}





