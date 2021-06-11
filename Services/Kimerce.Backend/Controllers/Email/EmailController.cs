using Kimerce.Backend.Dto.Items.Email;
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
    public class EmailController: ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("search")]
        public IActionResult Search([FromBody]SmartTableParam param)
        {
            var products = _emailService.Search(param);
            return Ok(products);
        }

        [HttpGet("getbyid/{id}")]
        public EmailItem Get(long id)
        {
            return _emailService.Get(id);
        }

        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]EmailItem account)
        {
            var result = await _emailService.CreateOrUpdate(account);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _emailService.Delete(id);
            return Ok(result);
        }
    }
}
