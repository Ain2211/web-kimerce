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
    public class CampaignOrderController : ControllerBase
    {

        private readonly ICampaignOrderService _campaignOrderService;

        public CampaignOrderController(ICampaignOrderService campaignOrderService)
        {
            _campaignOrderService = campaignOrderService;
        }

        /// <summary>
        /// Tìm kiếm chiến dịch
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var campaignOrders = await _campaignOrderService.Search(param);
            return Ok(campaignOrders);
        }

        /// <summary>
        /// Tạo / cập nhật chiến dịch
        /// </summary>
        /// <param name="campaignOrder"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]CampaignOrderModel campaignOrder)
        {
            var result = await _campaignOrderService.CreateOrUpdate(campaignOrder);
            return Ok(result);
        }


        /// <summary>
        /// Lấy thông tin chiến dịch
        /// </summary>
        /// <param name="id">Id của chiến dịch</param>
        /// <returns>CampaignOrder object</returns>
        [HttpGet("GetById/{id}")]
        public CampaignOrderModel Get(int id)
        {
            return _campaignOrderService.Get(id);
        }




        /// <summary>
        /// Xóa chiến dịch
        /// </summary>
        /// <param name="id">Mã chiến dịch cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _campaignOrderService.Delete(id);
            return Ok(result);
        }
    }
}