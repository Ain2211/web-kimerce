using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.Orders;
using Kimerce.Backend.Dto.Models.Attributes;
using Kimerce.Backend.Infrastructure.Services.SpecAttributes;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa thuộc tính kỹ thuật
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class SpecAttributeController : ControllerBase
    {

        private readonly ISpecAttributeService _specAttributeService;

        public SpecAttributeController(ISpecAttributeService specAttributeService)
        {
            _specAttributeService = specAttributeService;
        }

        /// <summary>
        /// Tìm kiếm thuộc tính kỹ thuật
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var SpecAttributes = await _specAttributeService.Search(param);
            return Ok(SpecAttributes);
        }

        /// <summary>
        /// Tạo / cập nhật thuộc tính kỹ thuật
        /// </summary>
        /// <param name="SpecAttribute"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]SpecAttributeModel SpecAttribute)
        {
            var result = await _specAttributeService.CreateOrUpdate(SpecAttribute);
            return Ok(result);
        }


        /// <summary>
        /// Lấy thông tin thuộc tính kỹ thuật
        /// </summary>
        /// <param name="id">Id của sản phẩm</param>
        /// <returns>SpecAttribute object</returns>
        [HttpGet("GetById/{id}")]
        public SpecAttributeModel Get(int id)
        {
            return _specAttributeService.Get(id);
        }


        /// <summary>
        /// Xóa thuộc tính kỹ thuật
        /// </summary>
        /// <param name="id">Mã sản phẩm cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _specAttributeService.Delete(id);
            return Ok(result);
        }
    }
}