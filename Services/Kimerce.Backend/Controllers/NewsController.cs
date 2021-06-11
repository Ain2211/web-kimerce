using Kimerce.Backend.Domain.News;
using Kimerce.Backend.Dto.Items.News;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Services.Categories;
using Kimerce.Backend.Infrastructure.Services.New;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa blog
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _service;
        public NewsController(INewsService service)
        {
            _service = service;
        }
        /// <summary>
        /// Tìm kiếm blog
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var news = await _service.Search(param);
            return Ok(news);
        }

        /// <summary>
        /// Tạo / cập nhật blog
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]NewsModel news)
        {
            var result = await _service.CreateOrUpdate(news);
            return Ok(result);
        }


        /// <summary>
        /// Lấy thông tin blog
        /// </summary>
        /// <param name="id">Id của blog</param>
        /// <returns>Blog object</returns>
        [HttpGet("GetById/{id}")]
        public News Get(int id)
        {
            return _service.GetById(id);
        }

        //[HttpGet("GetAll")]
        //public IQueryable<News> GetAll()
        //{
        //    return _service.Index();
        //}

        /// <summary>
        /// Xóa blog
        /// </summary>
        /// <param name="id">Mã blog cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }

    }
}
