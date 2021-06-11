using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Infrastructure.Data;
using Kimerce.Backend.Infrastructure.Services.Images;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.SmartTable;
using Kimerce.Backend.Dto.Models.Images;

namespace Kimerce.Backend.Controllers.Orders
{
    [Route("[controller]")]

    [ApiController]
    public class ImagesController : ControllerBase
    {

        private readonly IImageService _Service;


        public ImagesController(IImageService Service)
        {
            this._Service = Service;
        }

        
        //[HttpGet("GetAll")]

        //public async Task<ActionResult<IEnumerable<Image>>> GetAll()
        //{
        //    return await _Service.Index();
        //}


        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var image = await _Service.Search(param);
            return Ok(image);
        }

        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]ImageModel image)
        {
            var result = await _Service.CreateOrUpdate(image);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public Image Get(int id)
        {
            return _Service.Get(id);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _Service.Delete(id);
            return Ok(result);
        }
    }
}
