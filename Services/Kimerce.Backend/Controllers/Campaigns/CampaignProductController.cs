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
    public class CampaignProductController : ControllerBase
    {

        private readonly ICampaignProductService _campaignProductService;

        public CampaignProductController(ICampaignProductService campaignProductService)
        {
            _campaignProductService = campaignProductService;
        }

        /// <summary>
        /// Tìm kiếm chiến dịch
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var campaignProducts = await _campaignProductService.Search(param);
            return Ok(campaignProducts);
        }

        /// <summary>
        /// Tạo / cập nhật chiến dịch
        /// </summary>
        /// <param name="campaignProduct"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]CampaignProductModel campaignProduct)
        {
            var result = await _campaignProductService.CreateOrUpdate(campaignProduct);
            return Ok(result);
        }


        /// <summary>
        /// Lấy thông tin chiến dịch
        /// </summary>
        /// <param name="id">Id của chiến dịch</param>
        /// <returns>CampaignProduct object</returns>
        [HttpGet("GetById/{id}")]
        public CampaignProductModel Get(int id)
        {
            return _campaignProductService.Get(id);
        }




        /// <summary>
        /// Xóa chiến dịch
        /// </summary>
        /// <param name="id">Mã chiến dịch cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _campaignProductService.Delete(id);
            return Ok(result);
        }
    }
}