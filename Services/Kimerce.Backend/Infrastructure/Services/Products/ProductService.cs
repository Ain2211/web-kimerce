using System;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Domain.Products;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.Orders;
using Kimerce.Backend.Dto.Items.Products;
using Kimerce.Backend.Dto.Models.Products;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.EntityFrameworkCore;

namespace Kimerce.Backend.Infrastructure.Services.Products
{
    public interface IProductService
    {
        Task<SmartTableResult<ProductItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(ProductModel model, int updateBy = 0, string updateByUserName = "");
        ProductModel Get(int id);
        Task<BaseResult> Delete(int id);

        IQueryable<ProductItem> GetRelate(int id);
    }

    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<RelateProduct> _relateProductRepository;
        private readonly IRepository<ProductImage> _productImageRepository;
        ////private readonly IRepository<OrderItem> _orderItemRepository;

        public ProductService(IRepository<Product> productRepository, IRepository<RelateProduct> relateProductRepository, IRepository<ProductImage> productImageRepository)
        {
            _productRepository = productRepository;
            _relateProductRepository = relateProductRepository;
            _productImageRepository = productImageRepository;
        }

        public async Task<SmartTableResult<ProductItem>> Search(SmartTableParam param)
        {
            var query = _productRepository.Query();
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


            //param.Sort = new Sort() { Predicate = "Id", Reverse = false };
            var gridData = query.ToSmartTableResult(param, x => x.ToItem());
            return gridData;
        }

        public IQueryable<ProductItem> GetRelate(int id)
        {
            var relateparent = _relateProductRepository.Query().Where(x => x.ProductIdA == id).Select(x => x.ProductB);
            var relatechildren = _relateProductRepository.Query().Where(x => x.ProductIdB == id).Select(x => x.ProductA);
            var relate = relateparent.Concat(relatechildren);
            return relate.Select(x => x.ToItem());
        }

        #region CRUD

        public ProductModel Get(int id)
        {
            return id > 0 ? _productRepository.GetById(id).ToModel() : new ProductModel();

        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var productForDelete = _productRepository.GetById(id);
            if (productForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có thì xóa làm sao (o_ _)ﾉ彡☆";
                return result;
            }
            try
            {
                await _productRepository.DeleteAsync(productForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }



        public async Task<BaseResult> CreateOrUpdate(ProductModel model, int updateBy = 0, string updateByUserName = "")
        {
            var product = model.ToProduct();

            //Cập nhật thông tin chung của thực thể
            product = product.UpdateCommonInt(updateBy, updateByUserName);

            if (product.Id > 0)
            {
                //Cập nhật
                return await Update(product);
            }
            else
            {
                //Thêm mới
                return await Create(product);
            }
        }

        private async Task<BaseResult> Update(Product product, int updateBy = 0, string updateByUserName = "")
        {
            var result = new CreateOrUpdateResultInt() { Result = Result.Success };
            var productForUpdate = _productRepository.Query().FirstOrDefault(p => p.Id == product.Id);
            if (productForUpdate == null || product.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy sản phẩm yêu cầu!";
                return result;
            }
            try
            {
                productForUpdate = product.ToProduct(productForUpdate);

                //Cập nhật thông tin chung của thực thể
                productForUpdate = productForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await _productRepository.UpdateAsync(productForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = product.Id;
            return result;
        }

        private async Task<BaseResult> Create(Product product)
        {
            var result = new CreateOrUpdateResultInt();
            product.Name = product.Name.Trim();
            try
            {
                await _productRepository.InsertAsync(product);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = product.Id;
            return result;
        }


        #endregion
    }
}