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
    public class DiscountOrderController : ControllerBase
    {

        private readonly IDiscountOrderService _discountOrderService;

        public DiscountOrderController(IDiscountOrderService discountOrderService)
        {
            _discountOrderService = discountOrderService;
        }

        /// <summary>
        /// Tìm kiếm chiến dịch
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var discountOrders = await _discountOrderService.Search(param);
            return Ok(discountOrders);
        }

        /// <summary>
        /// Tạo / cập nhật chiến dịch
        /// </summary>
        /// <param name="discountOrder"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]DiscountOrderModel discountOrder)
        {
            var result = await _discountOrderService.CreateOrUpdate(discountOrder);
            return Ok(result);
        }


        /// <summary>
        /// Lấy thông tin chiến dịch
        /// </summary>
        /// <param name="id">Id của chiến dịch</param>
        /// <returns>DiscountOrder object</returns>
        [HttpGet("GetById/{id}")]
        public DiscountOrderModel Get(int id)
        {
            return _discountOrderService.Get(id);
        }




        /// <summary>
        /// Xóa chiến dịch
        /// </summary>
        /// <param name="id">Mã chiến dịch cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _discountOrderService.Delete(id);
            return Ok(result);
        }
    }
}