using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Infrastructure.Data;
using Kimerce.Backend.Infrastructure.Services.Categories;
using Kimerce.Backend.Domain.Locations;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.SmartTable;
using Kimerce.Backend.Dto.Models.Categories;
using Kimerce.Backend.Dto.Items.Locations;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa thành phố
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IHostingEnvironment _env;

        public CitiesController(ICityService cityService, IHostingEnvironment env)
        {
            _cityService = cityService;
            _env = env;
        }

        /// <summary>
        /// Tìm kiếm thành phố
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var cities = await _cityService.Search(param);
            return Ok(cities);
        }


        /// <summary>
        /// Tạo / cập nhật thành phố
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]CityModel city)
        {
            var result = await _cityService.CreateOrUpdate(city);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông tin thành phố
        /// </summary>
        /// <param name="id">Id của thành phố</param>
        /// <returns>City object</returns>
        [HttpGet("GetById/{id}")]
        public CityModel Get(int id)
        {
            return _cityService.Get(id);
        }

        /// <summary>
        /// Lấy danh sách huyện thuộc thành phố
        /// </summary>
        /// <param name="id">Id của thành phố</param>
        /// <returns></returns>
        [HttpGet("GetChildren/{id}")]
        public IQueryable<DistrictItem> GetChildren(int id)
        {
            return _cityService.GetChildren(id);
        }

        /// <summary>
        /// Xóa thành phố
        /// </summary>
        /// <param name="id">Mã thành phố cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _cityService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// Xuất ra file excel
        /// </summary>
        /// <param name="param">Danh sách</param>
        /// <returns></returns>
        [HttpPost("ExportToXlsx")]
        public async Task<IActionResult> ExportToXlsxAsync([FromBody]SmartTableParam param)
        {
            var result = await _cityService.ExportToXlsx(param);
            return Ok(result);
        }

        /// <summary>
        /// Download file excel
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet("download")]
        public async Task<ActionResult> Download()
        {
            string contentRootPath = _env.ContentRootPath;
            string fileName = "ExportFile.xlsx";
            string filePath = Path.Combine(contentRootPath, fileName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Path.GetFileName(filePath));
        }
    }
}
