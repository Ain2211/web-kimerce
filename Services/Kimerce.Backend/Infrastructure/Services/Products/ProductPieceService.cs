using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.EntityFrameworkCore;
using System;
using Kimerce.Backend.Dto.Items.Products;
using Kimerce.Backend.Dto.Models.Products;
using Kimerce.Backend.Domain.Products;

namespace Kimerce.Backend.Infrastructure.Services.ProductPieces
{
    public interface IProductPieceService
    {
        Task<SmartTableResult<ProductPieceItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(ProductPieceModel model, int updateBy = 0, string updateByUserName = "");
        ProductPieceModel Get(long id);
        Task<BaseResult> Delete(int id);
    }

    public class ProductPieceService : IProductPieceService
    {
        private readonly IRepository<ProductPiece> _ProductPieceRepository;

        public ProductPieceService(IRepository<ProductPiece> ProductPieceRepository)
        {
            _ProductPieceRepository = ProductPieceRepository;
        }

        public async Task<SmartTableResult<ProductPieceItem>> Search(SmartTableParam param)
        {
            var query = _ProductPieceRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.ProductId.ToString().Contains(keyword));
                }
                if (search.CreateStart != null)
                {
                    DateTime createStart = DateTime.Parse(search.CreateStart.ToString());
                    DateTime startOfDay = createStart.StartOfDay();
                    query = query.Where(x => x.CreatedTime >= startOfDay);
                }

                if (search.CreateEnd != null)
                {
                    DateTime createEnd = DateTime.Parse(search.CreateEnd.ToString());
                    DateTime endOfDay = createEnd.EndOfDay();
                    query = query.Where(x => x.CreatedTime <= endOfDay);
                }
            }


            //param.Sort = new Sort() { Predicate = "Id", Reverse = false };
            var gridData = query.ToSmartTableResult(param, x => x.ToItem());
            return gridData;
        }


        #region CRUD

        public ProductPieceModel Get(long id)
        {
            return id > 0 ? _ProductPieceRepository.GetById(id).ToModel() : new ProductPieceModel();

        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var ProductPieceForDelete = _ProductPieceRepository.GetById(id);
            if (ProductPieceForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có thì xóa làm sao (o_ _)ﾉ彡☆";
                return result;
            }
            try
            {
                await _ProductPieceRepository.DeleteAsync(ProductPieceForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }



        public async Task<BaseResult> CreateOrUpdate(ProductPieceModel model, int updateBy = 0, string updateByUserName = "")
        {
            var ProductPiece = model.ToProductPiece();

            //Cập nhật thông tin chung của thực thể
            ProductPiece = ProductPiece.UpdateCommonLong(updateBy, updateByUserName);

            if (ProductPiece.Id > 0)
            {
                //Cập nhật
                return await Update(ProductPiece);
            }
            else
            {
                //Thêm mới
                return await Create(ProductPiece);
            }
        }

        private async Task<BaseResult> Update(ProductPiece ProductPiece, int updateBy = 0, string updateByUserName = "")
        {
            var result = new CreateOrUpdateResultLong() { Result = Result.Success };
            var ProductPieceForUpdate = _ProductPieceRepository.Query().FirstOrDefault(p => p.Id == ProductPiece.Id);
            if (ProductPieceForUpdate == null || ProductPiece.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy sản phẩm yêu cầu!";
                return result;
            }
            try
            {
                ProductPieceForUpdate = ProductPiece.ToProductPiece(ProductPieceForUpdate);

                //Cập nhật thông tin chung của thực thể
                ProductPieceForUpdate = ProductPieceForUpdate.UpdateCommonLong(updateBy, updateByUserName);

                await _ProductPieceRepository.UpdateAsync(ProductPieceForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = ProductPiece.Id;
            return result;
        }

        private async Task<BaseResult> Create(ProductPiece ProductPiece)
        {
            var result = new CreateOrUpdateResultLong();
            try
            {
                await _ProductPieceRepository.InsertAsync(ProductPiece);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = ProductPiece.Id;
            return result;
        }


        #endregion
    }
}