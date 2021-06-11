using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.News;
using Kimerce.Backend.Dto.Models.News;
using Kimerce.Backend.Infrastructure.Services.NewImage;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa blog liên quan
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    
    public class NewsImageController : ControllerBase
    {
        private readonly INewsImageService _newsImageService;

        public NewsImageController(INewsImageService newsImageService)
        {
            _newsImageService = newsImageService;
        }

        /// <summary>
        /// Tìm kiếm ảnh 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var NewsImages = await _newsImageService.Search(param);
            return Ok(NewsImages);
        }

        /// <summary>
        /// Lấy ảnh cụ thể của 1 blog
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetImageByNews/{id}")]
        public IQueryable<ImageItem> GetImageByNews(int id)
        {
            var image = _newsImageService.GetImageByNews(id);
            return image;
        }

        [HttpGet("GetNewsImage/{id}")]
        public IQueryable<NewsImageItem> GetNewsImage(int id)
        {
            var newsImage = _newsImageService.GetNewsImage(id);
            return newsImage;
        }

        /// <summary>
        /// Lấy thông tin liên kết ảnh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public NewsImageModel Get(int id)
        {
            return _newsImageService.Get(id);
        }

        /// <summary>
        /// Tạo / Cập nhật liên kết ảnh
        /// </summary>
        /// <param name="ProductImage"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]NewsImageModel newsImage)
        {
            var result = await _newsImageService.CreateOrUpdate(newsImage);
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
            var result = await _newsImageService.Delete(id);
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
            var result = await _newsImageService.Delete(id1, id2);
            return Ok(result);
        }
    }
}
