using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.Orders;
using Kimerce.Backend.Dto.Models.Attributes;
using Kimerce.Backend.Infrastructure.Services.InventoryAttributes;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa thuộc tính kho
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class InventoryAttributeController : ControllerBase
    {

        private readonly IInventoryAttributeService _inventoryAttributeService;

        public InventoryAttributeController(IInventoryAttributeService inventoryAttributeService)
        {
            _inventoryAttributeService = inventoryAttributeService;
        }

        /// <summary>
        /// Tìm kiếm thuộc tính kho
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var InventoryAttributes = await _inventoryAttributeService.Search(param);
            return Ok(InventoryAttributes);
        }

        /// <summary>
        /// Tạo / cập nhật thuộc tính kho
        /// </summary>
        /// <param name="InventoryAttribute"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]InventoryAttributeModel InventoryAttribute)
        {
            var result = await _inventoryAttributeService.CreateOrUpdate(InventoryAttribute);
            return Ok(result);
        }


        /// <summary>
        /// Lấy thông tin thuộc tính kho
        /// </summary>
        /// <param name="id">Id của sản phẩm</param>
        /// <returns>InventoryAttribute object</returns>
        [HttpGet("GetById/{id}")]
        public InventoryAttributeModel Get(int id)
        {
            return _inventoryAttributeService.Get(id);
        }


        /// <summary>
        /// Xóa thuộc tính kho
        /// </summary>
        /// <param name="id">Mã sản phẩm cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _inventoryAttributeService.Delete(id);
            return Ok(result);
        }
    }
}