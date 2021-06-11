using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Shipments;
using Kimerce.Backend.Dto.Items.Shipments;
using Kimerce.Backend.Dto.Models.Shipments;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Services.Shipments;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kimerce.Backend.Controllers.Shipments
{
    /// <summary>
    /// Thêm, Sửa, Xóa đơn hàng
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ShipmentsController : Controller
    {
        private readonly ShipmentService _shipmentService;
        public ShipmentsController(ShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }
        /// <summary>
        /// Tìm kiếm đơn hàng
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var shipments = await _shipmentService.Search(param);
            return Ok(shipments);
        }

        /// <summary>
        /// Chi tiết đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        [HttpGet("GetById/{id}")]
        public async Task<Shipment_Item> Get(int id)
        {
            return await _shipmentService.GetbyId(id);
        }

        /// <summary>
        /// Tạo / Cập nhật đơn hàng
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]ShipmentModel shipment)
        {
            var res = await _shipmentService.CreateOrUpdate(shipment);
            return Ok(res);
        }

        /// <summary>
        /// Xóa đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<controller>/5
        [HttpDelete("Delete/{id}")]
        public BaseResult Delete(int id)
        {
            return _shipmentService.Delete(id);
        }
    }
}
