using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kimerce.Backend.Domain.Locations;
using Kimerce.Backend.Infrastructure.Data;
using Kimerce.Backend.Infrastructure.Services.Categories;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.SmartTable;
using Kimerce.Backend.Dto.Models.Categories;
using Kimerce.Backend.Dto.Items.Locations;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa quận
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class DistrictsController : ControllerBase
    {
        private readonly IDistrictService _districtService;
        private readonly IHostingEnvironment _env;

        public DistrictsController(IDistrictService districtService, IHostingEnvironment env)
        {
            _districtService = districtService;
            _env = env;
        }

        /// <summary>
        /// Tìm kiếm quận
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var districts = await _districtService.Search(param);
            return Ok(districts);
        }


        /// <summary>
        /// Tạo / cập nhật quận
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]DistrictModel District)
        {
            var result = await _districtService.CreateOrUpdate(District);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông tin quận
        /// </summary>
        /// <param name="id">Id của quận</param>
        /// <returns>District object</returns>
        [HttpGet("GetById/{id}")]
        public DistrictModel Get(int id)
        {
            return _districtService.Get(id);
        }

        /// <summary>
        /// Lấy danh sách xã thuộc huyện
        /// </summary>
        /// <param name="id">Id của huyện</param>
        /// <returns></returns>
        [HttpGet("GetChildren/{id}")]
        public IQueryable<WardItem> GetChildren(int id)
        {
            return _districtService.GetChildren(id);
        }

        /// <summary>
        /// Xóa quận
        /// </summary>
        /// <param name="id">Mã quận cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _districtService.Delete(id);
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
            var result = await _districtService.ExportToXlsx(param);
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
