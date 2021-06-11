
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
    public class EmailAccountController : ControllerBase
    {
        private readonly IEmailAccountService _emailAccountService;

        public EmailAccountController(IEmailAccountService emailAccountService)
        {
            _emailAccountService = emailAccountService;
        }

        [HttpPost("search")]
        public IActionResult Search([FromBody]SmartTableParam param)
        {
            var products = _emailAccountService.Search(param);
            return Ok(products);
        }

        [HttpGet("getbyid/{id}")]
        public EmailAccountItem Get(long id)
        {
            return _emailAccountService.Get(id);
        }

        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]EmailAccountItem account)
        {
            var result = await _emailAccountService.CreateOrUpdate(account);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _emailAccountService.Delete(id);
            return Ok(result);
        }
        [HttpGet("getemails/{id}")]
        public IQueryable<EmailItem> GetEmails(long id)
        {
            return _emailAccountService.GetEmails(id);
        }
    }

}
