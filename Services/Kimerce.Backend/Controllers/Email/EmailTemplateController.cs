
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
    public class EmailTemplateController : ControllerBase
    {
        private readonly IEmailTemplateService _emailTemplateService;

        public EmailTemplateController(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }

        [HttpPost("search")]
        public IActionResult Search([FromBody]SmartTableParam param)
        {
            var products = _emailTemplateService.Search(param);
            return Ok(products);
        }

        [HttpGet("getbyid/{id}")]
        public EmailTemplateItem Get(long id)
        {
            return _emailTemplateService.Get(id);
        }

        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]EmailTemplateItem template)
        {
            var result = await _emailTemplateService.CreateOrUpdate(template);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<BaseResult> Delete(long id)
        {
            var result = await _emailTemplateService.Delete(id);
            return result;
        }
        [HttpGet("getemails/{id}")]
        public IQueryable<EmailItem> GetEmails(long id)
        {
            return _emailTemplateService.GetEmails(id);
        }
    }

}
