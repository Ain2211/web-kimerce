using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Shipments;
using Kimerce.Backend.Dto.Models.Shipments;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Services.ShipmentItems;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kimerce.Backend.Controllers.Shipments
{
    /// <summary>
    /// Thêm, sửa, xóa item đơn vận
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ShipmentItemsController : Controller
    {
        private readonly ShipmentItemService _shipmentItemService;
        public ShipmentItemsController(ShipmentItemService shipmentItemService)
        {
            _shipmentItemService = shipmentItemService;
        }
        /// <summary>
        /// Tìm kiếm đơn item vận
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var shipmentItems = await _shipmentItemService.Search(param);
            return Ok(shipmentItems);
        }

        /// <summary>
        /// Chi tiết đơn item vận
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        [HttpGet("GetById/{id}")]
        public async Task<ShipmentItem> GetById(int Id)
        {
            return await _shipmentItemService.GetbyId(Id);
        }
        /// <summary>
        /// Tạo / Cập nhật item đơn vận
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]ShipmentItemModel model)
        {
            var res = await _shipmentItemService.CreateOrUpdate(model);
            return Ok(res);
        }
        

       
        /// <summary>
        /// Xóa đơn item vận
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        // DELETE api/<controller>/5
        [HttpDelete("Delete/{id}")]
        public BaseResult Delete(int Id)
        {
            return _shipmentItemService.Delete(Id);
        }
    }
}
