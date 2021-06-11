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
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Dto.Models.Products;
using Kimerce.Backend.Infrastructure.SmartTable;
using Kimerce.Backend.Dto.Items.Products;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa danh mục sản phẩm
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        /// <summary>
        /// Tìm kiếm danh mục sản phẩm
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var result = await _productCategoryService.Search(param);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông tin sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public ProductCategoryModel Get(int id)
        {
            return _productCategoryService.Get(id);
        }

        /// <summary>
        /// Tạo / Cập nhật sản phẩm
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]ProductCategoryModel productCategory)
        {
            var result = await _productCategoryService.CreateOrUpdate(productCategory);
            return Ok(result);
        }

        /// <summary>
        /// Xóa sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productCategoryService.Delete(id);
            return Ok(result);
        }

        [HttpGet("GetChildren/{id}")]
        public IQueryable<ProductCategoryItem> GetChildren(int id)
        {
            return _productCategoryService.GetChildren(id);
        }
    }
}
