
using Kimerce.Backend.Domain.Email;
using Kimerce.Backend.Dto.Items.Campaigns;
using Kimerce.Backend.Dto.Items.Email;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Services.Campaigns;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Controllers.Campaigns
{
    [Route("[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpPost("Search")]
        public IActionResult Search([FromBody]SmartTableParam param)
        {
            var products = _campaignService.Search(param);
            return Ok(products);
        }

        [HttpGet("GetById/{id}")]
        public CampaignItem Get(int id)
        {
            return _campaignService.Get(id);
        }

        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]CampaignItem campaign)
        {
            var result = await _campaignService.CreateOrUpdate(campaign);
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _campaignService.Delete(id);
            return Ok(result);
        }
    }

}
