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
using Microsoft.EntityFrameworkCore;

namespace Kimerce.Backend.Infrastructure.Services.Products
{
    public interface IProductCategoryService
    {
        Task<SmartTableResult<ProductCategoryItem>> Search(SmartTableParam param);
        ProductCategoryModel Get(int id);
        Task<BaseResult> Delete(int id);
        Task<BaseResult> CreateOrUpdate(ProductCategoryModel model, int updateBy = 0, string updateByUserName = "");
        IQueryable<ProductCategoryItem> GetChildren(int id);
    }

    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IRepository<ProductCategory> _productCategoryRepository;

        public ProductCategoryService(IRepository<ProductCategory> productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<SmartTableResult<ProductCategoryItem>> Search(SmartTableParam param)
        {
            var query = _productCategoryRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.Name.Contains(keyword));
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

        public IQueryable<ProductCategoryItem> GetChildren(int id)
        {
            return id > 0 ? _productCategoryRepository.Query().Where(x => x.ParentId == id).Select(x => x.ToItem())
                          : _productCategoryRepository.Query().Where(x => x.ParentId == null).Select(x => x.ToItem());
        }

        #region CRUD

        public ProductCategoryModel Get(int id)
        {
            return id > 0 ? _productCategoryRepository.GetById(id).ToModel() : new ProductCategoryModel();
        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var productCategoryForDelete = _productCategoryRepository.GetById(id);
            if (productCategoryForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có thì xóa làm sao (o_ _)ﾉ彡☆";
                return result;
            }
            try
            {
                await _productCategoryRepository.DeleteAsync(productCategoryForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        public async Task<BaseResult> CreateOrUpdate(ProductCategoryModel model, int updateBy = 0, string updateByUserName = "")
        {
            var productCategory = model.ToProductCategory();
            productCategory = productCategory.UpdateCommonInt(updateBy, updateByUserName);

            if (productCategory.Id > 0)
            {
                return await Update(productCategory);
            }
            else
            {
                return await Create(productCategory);
            }
        }

        private async Task<BaseResult> Create(ProductCategory productCategory)
        {
            var result = new CreateOrUpdateResultInt() { Result = Result.Success };
            productCategory.Name = productCategory.Name.Trim();
            try
            {
                await _productCategoryRepository.InsertAsync(productCategory);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = productCategory.Id;
            return result;
        }

        private async Task<BaseResult> Update(ProductCategory productCategory, int updateBy = 0, string updateByUserName = "")
        {
            var result = new CreateOrUpdateResultInt() { Result = Result.Success };
            var productCategoryForUpdate = _productCategoryRepository.Query().FirstOrDefault(c => c.Id == productCategory.Id);
            if (productCategoryForUpdate == null || productCategoryForUpdate.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy sản phẩm yêu cầu";
                return result;
            }
            try
            {
                productCategoryForUpdate = productCategory.ToProductCategory(productCategoryForUpdate);

                productCategoryForUpdate.UpdateCommonInt(updateBy, updateByUserName);
                await _productCategoryRepository.UpdateAsync(productCategoryForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = productCategory.Id;
            return result;
        }

        #endregion
    }
}