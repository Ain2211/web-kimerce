using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Products;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Products;
using Kimerce.Backend.Dto.Models.Products;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;

namespace Kimerce.Backend.Infrastructure.Services.Products
{
    public interface IRelateProductService
    {
        Task<SmartTableResult<RelateProductItem>> Search(SmartTableParam param);
        RelateProductModel Get(int id);
        Task<BaseResult> CreateOrUpdate(RelateProductModel model);
        Task<BaseResult> Delete(int id);
        Task<BaseResult> Delete(int id1, int id2);
    }

    public class RelateProductService : IRelateProductService
    {
        private readonly IRepository<RelateProduct> _relateProductRepository;
        private readonly IRepository<Product> _productRepository;

        public RelateProductService(IRepository<RelateProduct> relateProductRepository, IRepository<Product> productRepository)
        {
            _relateProductRepository = relateProductRepository;
            _productRepository = productRepository;
        }

        public async Task<SmartTableResult<RelateProductItem>> Search(SmartTableParam param)
        {
            var query = _relateProductRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.ProductIdA.ToString().Contains(keyword) || x.ProductIdB.ToString().Contains(keyword));
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
            //param.Sort = new Sort() { Predicate = "DisplayOrder", Reverse = false };
            var gridData = query.ToSmartTableResult(param, x => x.ToItem());
            return gridData;
        }


        #region CRUD

        public RelateProductModel Get(int id)
        {
            return id > 0 ? _relateProductRepository.GetById(id).ToModel() : new RelateProductModel();
        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var productForDelete = _relateProductRepository.GetById(id);
            if (productForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có thì xóa làm sao (o_ _)ﾉ彡☆";
                return result;
            }
            try
            {
                await _relateProductRepository.DeleteAsync(productForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        public async Task<BaseResult> Delete(int id1, int id2)
        {
            var result = new BaseResult() { Result = Result.Success };
            var productForDelete = _relateProductRepository.Query().FirstOrDefault(x => (x.ProductIdA == id1 && x.ProductIdB == id2)
            || (x.ProductIdB == id1 && x.ProductIdA == id2));
            if (productForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có thì xóa làm sao (o_ _)ﾉ彡☆";
                return result;
            }
            try
            {
                await _relateProductRepository.DeleteAsync(productForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }



        public async Task<BaseResult> CreateOrUpdate(RelateProductModel model)
        {
            var result = new BaseResult();
            var relateProduct = model.ToRelateProduct();

            //Cập nhật thông tin chung của thực thể
            relateProduct = relateProduct.UpdateCommonInt();

            // Kiểm tra A và B
            if (relateProduct.ProductIdA == relateProduct.ProductIdB)
            {
                result.Result = Result.Failed;
                result.Message = "A và B bị trùng!";
                return result;
            }
            // kiểm tra trùng lặp trong dtb
            var query = _relateProductRepository.Query().FirstOrDefault(
                x => (x.ProductIdA == relateProduct.ProductIdA && x.ProductIdB == relateProduct.ProductIdB) || (x.ProductIdB == relateProduct.ProductIdA && x.ProductIdA == relateProduct.ProductIdB));
            if (query != null)
            {
                result.Result = Result.Failed;
                result.Message = "Sản phẩm liên quan đã tồn tại!";
                return result;
            }
            // Kiểm tra xem A và B có tồn tại hay không
            var isProductAExist = _productRepository.Query().FirstOrDefault(
                x => x.Id == relateProduct.ProductIdA);
            var isProductBExist = _productRepository.Query().FirstOrDefault(
                x => x.Id == relateProduct.ProductIdB);
            if (isProductAExist == null)
            {
                result.Result = Result.Failed;
                result.Message = "Sản phẩm A không tồn tại!";
                return result;
            }
            if (isProductBExist == null)
            {
                result.Result = Result.Failed;
                result.Message = "Sản phẩm B không tồn tại!";
                return result;
            }

            if (relateProduct.Id > 0)
            {
                //Cập nhật
                return await Update(relateProduct);
            }
            else
            {
                //Thêm mới
                return await Create(relateProduct);
            }
        }

        private async Task<BaseResult> Update(RelateProduct relateProduct)
        {
            var result = new BaseResult() { Result = Result.Success };
            var relateProductForUpdate = _relateProductRepository.Query().FirstOrDefault(p => p.Id == relateProduct.Id);
            if (relateProductForUpdate == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy sản phẩm liên quan yêu cầu!";
                return result;
            }
            try
            {
                relateProductForUpdate = relateProduct.ToRelateProduct(relateProductForUpdate);

                //Cập nhật thông tin chung của thực thể
                relateProductForUpdate = relateProductForUpdate.UpdateCommonInt();

                await _relateProductRepository.UpdateAsync(relateProductForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        private async Task<BaseResult> Create(RelateProduct relateProduct)
        {
            var result = new CreateOrUpdateResultInt();
            try
            {
                await _relateProductRepository.InsertAsync(relateProduct);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = relateProduct.Id;
            return result;
        }


        #endregion
    }
}