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
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.Products;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa blog liên quan
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ProductImage : ControllerBase
    {
        private readonly IProductImageService _ProductImageService;

        public ProductImage(IProductImageService ProductImageService)
        {
            _ProductImageService = ProductImageService;
        }

        /// <summary>
        /// Tìm kiếm ảnh 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var ProductImages = await _ProductImageService.Search(param);
            return Ok(ProductImages);
        }

        /// <summary>
        /// Lấy ảnh cụ thể của 1 sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetImageByProduct/{id}")]
        public IQueryable<ImageItem> GetImageByProduct(int id)
        {
            var image = _ProductImageService.GetImageByProduct(id);
            return image;
        }

        [HttpGet("GetProductImage/{id}")]
        public IQueryable<ProductImageItem> GetProductImage(int id)
        {
            var productImage = _ProductImageService.GetProductImage(id);
            return productImage;
        }

        /// <summary>
        /// Lấy thông tin liên kết ảnh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public ProductImageModel Get(int id)
        {
            return _ProductImageService.Get(id);
        }

        /// <summary>
        /// Tạo / Cập nhật liên kết ảnh
        /// </summary>
        /// <param name="ProductImage"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]ProductImageModel ProductImage)
        {
            var result = await _ProductImageService.CreateOrUpdate(ProductImage);
            return Ok(result);
        }

        /// <summary>
        /// Xóa liên kết ảnh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ProductImageService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// Xóa liên kết ảnh
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id1}/{id2}")]
        public async Task<IActionResult> Delete(int id1, int id2)
        {
            var result = await _ProductImageService.Delete(id1, id2);
            return Ok(result);
        }
    }
}
