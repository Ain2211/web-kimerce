
using Kimerce.Backend.Domain.Email;
using Kimerce.Backend.Dto.Items.Email;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Services;
using Kimerce.Backend.Infrastructure.Services.EmailService;
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
    public class EmailProviderController : ControllerBase
    {
        private readonly IEmailProviderService _emailProviderService;

        public EmailProviderController(IEmailProviderService emailProviderService)
        {
            _emailProviderService = emailProviderService;
        }

        [HttpPost("search")]
        public IActionResult Search([FromBody]SmartTableParam param)
        {
            var products = _emailProviderService.Search(param);
            return Ok(products);
        }

        [HttpGet("getbyid/{id}")]
        public EmailProviderItem Get(long id)
        {
            return _emailProviderService.Get(id);
        }

        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]EmailProviderItem provider)
        {
            var result = await _emailProviderService.CreateOrUpdate(provider);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<BaseResult> Delete(long id)
        {
            var result = await _emailProviderService.Delete(id);
            return result;
        }

        [HttpGet("getaccounts/{id}")]
        public IQueryable<EmailAccountItem> GetAccounts(long id)
        {
            return _emailProviderService.GetAccounts(id);
        }
    }

}
