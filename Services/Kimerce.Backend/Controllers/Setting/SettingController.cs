using Kimerce.Backend.Domain.Setting;
using Kimerce.Backend.Dto.Items.Setting;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Services;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpPost("search")]
        public IActionResult Search([FromBody]SmartTableParam param)
        {
            var products = _settingService.Search(param);
            return Ok(products);
        }

        [HttpGet("getbyid/{id}")]
        public SettingItem Get(int id)
        {
            return _settingService.Get(id);
        }

        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]SettingItem setting)
        {
            var result = await _settingService.CreateOrUpdate(setting);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<BaseResult> Delete(int id)
        {
            var result = await _settingService.Delete(id);
            return result;
        }
    }

}
