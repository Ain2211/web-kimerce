
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Tracking;
using Kimerce.Backend.Dto.Models.Tracking;
using Kimerce.Backend.Infrastructure.Services.Trackings;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Mvc;

namespace Kimerce.Backend.Controllers.Trackings
{
    [Route("[controller]")]
    [ApiController]

    public class TrackingController : ControllerBase
    {

        private readonly ITrackingService _service;
        public TrackingController(ITrackingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Tìm kiếm lịch sử
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var products = await _service.Search(param);
            return Ok(products);
        }



        /// <summary>
        /// Tạo / cập nhật lịch sử
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>

        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]Tracking track)
        {
            var result = await _service.CreateOrUpdate(track);
            return Ok(result);
        }

        /// <summary>
        /// Lấy thông tin lịch sử
        /// </summary>
        /// <param name="id">Id của lịch sử</param>
        /// <returns>Product object</returns>
        [HttpGet("GetById/{id}")]
        public Tracking Get(int id)
        {
            return _service.GetId(id);
        }


        /// <summary>
        /// Xóa lịch sử
        /// </summary>
        /// <param name="id">Mã lịch sử cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }

    }
}