using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kimerce.Backend.Domain.Products;
using Kimerce.Backend.Infrastructure.Data;
using Kimerce.Backend.Infrastructure.Services.Products;
using Kimerce.Backend.Infrastructure.SmartTable;
using Kimerce.Backend.Dto.Models.Products;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa sản phẩm liên quan
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class RelateProductController : ControllerBase
    {
        private readonly IRelateProductService _relateProductService;

        public RelateProductController(IRelateProductService relateProductService)
        {
            _relateProductService = relateProductService;
        }

        /// <summary>
        /// Tìm kiếm sản phẩm liên quan
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var relateProducts = await _relateProductService.Search(param);
            return Ok(relateProducts);
        }

        /// <summary>
        /// Lấy thông tin sản phẩm liên quan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public RelateProductModel Get(int id)
        {
            return _relateProductService.Get(id);
        }

        /// <summary>
        /// Tạo / Cập nhật sản phẩm liên quan
        /// </summary>
        /// <param name="relateProduct"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]RelateProductModel relateProduct)
        {
            var result = await _relateProductService.CreateOrUpdate(relateProduct);
            return Ok(result);
        }

        /// <summary>
        /// Xóa sản phẩm liên quan bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _relateProductService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// Xóa sản phẩm liên quan
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id1}/{id2}")]
        public async Task<IActionResult> Delete(int id1, int id2)
        {
            var result = await _relateProductService.Delete(id1, id2);
            return Ok(result);
        }
    }
}
