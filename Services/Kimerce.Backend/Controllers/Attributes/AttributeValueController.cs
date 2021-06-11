using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.Orders;
using Kimerce.Backend.Dto.Models.Attributes;
using Kimerce.Backend.Infrastructure.Services.AttributeValues;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa giá trị thuộc tính
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AttributeValueController : ControllerBase
    {

        private readonly IAttributeValueService _attributeValueService;

        public AttributeValueController(IAttributeValueService attributeValueService)
        {
            _attributeValueService = attributeValueService;
        }

        /// <summary>
        /// Tìm kiếm giá trị thuộc tính
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var AttributeValues = await _attributeValueService.Search(param);
            return Ok(AttributeValues);
        }

        /// <summary>
        /// Tạo / cập nhật giá trị thuộc tính
        /// </summary>
        /// <param name="AttributeValue"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]AttributeValueModel AttributeValue)
        {
            var result = await _attributeValueService.CreateOrUpdate(AttributeValue);
            return Ok(result);
        }


        /// <summary>
        /// Lấy thông tin giá trị thuộc tính
        /// </summary>
        /// <param name="id">Id của sản phẩm</param>
        /// <returns>AttributeValue object</returns>
        [HttpGet("GetById/{id}")]
        public AttributeValueModel Get(int id)
        {
            return _attributeValueService.Get(id);
        }


        /// <summary>
        /// Xóa giá trị thuộc tính
        /// </summary>
        /// <param name="id">Mã sản phẩm cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _attributeValueService.Delete(id);
            return Ok(result);
        }
    }
}