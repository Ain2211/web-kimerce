using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Campaigns;
using Kimerce.Backend.Dto.Models.Campaigns;
using Kimerce.Backend.Infrastructure.Services.Campaigns;
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
    public class DiscountController : ControllerBase
    {

        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        /// <summary>
        /// Tìm kiếm giảm giá
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var discounts = await _discountService.Search(param);
            return Ok(discounts);
        }

        /// <summary>
        /// Tạo / cập nhật giảm giá
        /// </summary>
        /// <param name="discount"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]DiscountModel discount)
        {
            var result = await _discountService.CreateOrUpdate(discount);
            return Ok(result);
        }


        /// <summary>
        /// Lấy thông tin giảm giá
        /// </summary>
        /// <param name="id">Id của giảm giá</param>
        /// <returns>Discount object</returns>
        [HttpGet("GetById/{id}")]
        public DiscountModel Get(int id)
        {
            return _discountService.Get(id);
        }




        /// <summary>
        /// Xóa giảm giá
        /// </summary>
        /// <param name="id">Mã giảm giá cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _discountService.Delete(id);
            return Ok(result);
        }
    }
}