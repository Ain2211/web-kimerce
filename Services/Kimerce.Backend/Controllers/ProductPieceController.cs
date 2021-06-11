using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Dto.Items.Products;
using Kimerce.Backend.Dto.Models.Products;
using Kimerce.Backend.Infrastructure.Services.ProductPieces;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kimerce.Backend.Controllers
{
    /// <summary>
    /// API tìm kiếm, thêm, sửa, xóa đơn vị sản phẩm
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class ProductPieceController : ControllerBase
    {

        private readonly IProductPieceService _ProductPieceService;

        public ProductPieceController(IProductPieceService ProductPieceService)
        {
            _ProductPieceService = ProductPieceService;
        }

        /// <summary>
        /// Tìm kiếm đơn vị sản phẩm
        /// </summary>
        /// <param name="param">Tham số dạng smarttable</param>
        /// <returns></returns>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody]SmartTableParam param)
        {
            var ProductPieces = await _ProductPieceService.Search(param);
            return Ok(ProductPieces);
        }

        /// <summary>
        /// Tạo / cập nhật đơn vị sản phẩm
        /// </summary>
        /// <param name="ProductPiece"></param>
        /// <returns></returns>
        [HttpPost("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]ProductPieceModel ProductPiece)
        {
            var result = await _ProductPieceService.CreateOrUpdate(ProductPiece);
            return Ok(result);
        }


        /// <summary>
        /// Lấy thông tin đơn vị sản phẩm
        /// </summary>
        /// <param name="id">Id của sản phẩm</param>
        /// <returns>ProductPiece object</returns>
        [HttpGet("GetById/{id}")]
        public ProductPieceModel Get(long id)
        {
            return _ProductPieceService.Get(id);
        }


        /// <summary>
        /// Xóa đơn vị sản phẩm
        /// </summary>
        /// <param name="id">Mã sản phẩm cần xóa</param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ProductPieceService.Delete(id);
            return Ok(result);
        }
    }
}