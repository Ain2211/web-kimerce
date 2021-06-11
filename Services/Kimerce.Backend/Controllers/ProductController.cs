using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Products;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.Orders;
using Kimerce.Backend.Dto.Items.Products;
using Kimerce.Backend.Dto.Models.Products;
using Kimerce.Backend.Infrastructure.Services.Products;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa sản phẩm
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Lấy sản phẩm liên quan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetRelate/{id}")]
        public IQueryable<ProductItem> GetRelate( int id)
        {
            var relate = _productService.GetRelate(id);
            return relate;
        }

        /// <summary>
        /// Tìm kiếm sản phẩm
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var products = await _productService.Search(param);
            return Ok(products);
        }

        /// <summary>
        /// Tạo / cập nhật sản phẩm
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]ProductModel product)
        {
            var result = await _productService.CreateOrUpdate(product);
            return Ok(result);
        }


        /// <summary>
        /// Lấy thông tin sản phẩm
        /// </summary>
        /// <param name="id">Id của sản phẩm</param>
        /// <returns>Product object</returns>
        [HttpGet("GetById/{id}")]
        public ProductModel Get(int id)
        {
            return _productService.Get(id);
        }


        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        /// <param name="id">Mã sản phẩm cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);
            return Ok(result);
        }
    }
}