using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kimerce.Backend.Infrastructure.Services.Categories;
using Kimerce.Backend.Infrastructure.SmartTable;
using Kimerce.Backend.Dto.Models.Categories;
using Kimerce.Backend.Domain.WareHouse;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa nhà kho
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class WareHousesController : ControllerBase
    {
        private readonly IWareHouseService _wareHouseService;
        private readonly IHostingEnvironment _env;

        public WareHousesController(IWareHouseService wareHouseService, IHostingEnvironment env)
        {
            _wareHouseService = wareHouseService;
            _env = env;
        }

        /// <summary>
        /// Tìm kiếm nhà kho
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var wareHouses = await _wareHouseService.Search(param);
            return Ok(wareHouses);
        }


        /// <summary>
        /// Tạo / cập nhật nhà kho
        /// </summary>
        /// <param name="wareHouse"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]WareHouseModel wareHouse)
        {
            var result = await _wareHouseService.CreateOrUpdate(wareHouse);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông tin nhà kho
        /// </summary>
        /// <param name="id">Id của nhà kho</param>
        /// <returns>WareHouse object</returns>
        [HttpGet("GetById/{id}")]
        public WareHouseModel Get(int id)
        {
            return _wareHouseService.Get(id);
        }

        /// <summary>
        /// Xóa nhà kho
        /// </summary>
        /// <param name="id">Mã nhà kho cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _wareHouseService.Delete(id);
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
            var result = await _wareHouseService.ExportToXlsx(param);           
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
