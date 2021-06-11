using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Attributes;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.Orders;
using Kimerce.Backend.Dto.Items.Attributes;
using Kimerce.Backend.Dto.Models.Attributes;
using Kimerce.Backend.Infrastructure.Services.Attributes;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa thuộc tính sản phẩm
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AttributeController : ControllerBase
    {

        private readonly IAttributeService _attributeService;

        public AttributeController(IAttributeService AttributeService)
        {
            _attributeService = AttributeService;
        }

        /// <summary>
        /// Tìm kiếm thuộc tính sản phẩm
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var Attributes = await _attributeService.Search(param);
            return Ok(Attributes);
        }

        /// <summary>
        /// Tạo / cập nhật thuộc tính sản phẩm
        /// </summary>
        /// <param name="Attribute"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]AttributeModel Attribute)
        {
            var result = await _attributeService.CreateOrUpdate(Attribute);
            return Ok(result);
        }


        /// <summary>
        /// Lấy thông tin thuộc tính sản phẩm
        /// </summary>
        /// <param name="id">Id của sản phẩm</param>
        /// <returns>Attribute object</returns>
        [HttpGet("GetById/{id}")]
        public AttributeModel Get(int id)
        {
            return _attributeService.Get(id);
        }


        /// <summary>
        /// Xóa thuộc tính sản phẩm
        /// </summary>
        /// <param name="id">Mã sản phẩm cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _attributeService.Delete(id);
            return Ok(result);
        }
    }
}