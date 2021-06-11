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
using Kimerce.Backend.Dto.Items.Categories.WareHouse;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa phường
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class WardsController : ControllerBase
    {
        private readonly IWardService _wardService;
        private readonly IHostingEnvironment _env;

        public WardsController(IWardService wardService, IHostingEnvironment env)
        {
            _wardService = wardService;
            _env = env;
        }

        /// <summary>
        /// Tìm kiếm phường
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var wards = await _wardService.Search(param);
            return Ok(wards);
        }


        /// <summary>
        /// Tạo / cập nhật phường
        /// </summary>
        /// <param name="ward"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]WardModel ward)
        {
            var result = await _wardService.CreateOrUpdate(ward);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông tin phường
        /// </summary>
        /// <param name="id">Id của phường</param>
        /// <returns>Ward object</returns>
        [HttpGet("GetById/{id}")]
        public WardModel Get(int id)
        {
            return _wardService.Get(id);
        }

        /// <summary>
        /// Lấy danh sách nhà kho thuộc xã
        /// </summary>
        /// <param name="id">Id của xã</param>
        /// <returns></returns>
        [HttpGet("GetChildren/{id}")]
        public IQueryable<WareHouseItem> GetChildren(int id)
        {
            return _wardService.GetChildren(id);
        }

        /// <summary>
        /// Xóa phường
        /// </summary>
        /// <param name="id">Mã phường cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _wardService.Delete(id);
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
            var result = await _wardService.ExportToXlsx(param);
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
