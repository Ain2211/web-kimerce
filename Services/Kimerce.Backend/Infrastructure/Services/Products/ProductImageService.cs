using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Domain.Products;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.Products;
using Kimerce.Backend.Dto.Models.Products;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;

namespace Kimerce.Backend.Infrastructure.Services.Products
{
    public interface IProductImageService
    {
        Task<SmartTableResult<ProductImageItem>> Search(SmartTableParam param);
        ProductImageModel Get(int id);
        Task<BaseResult> CreateOrUpdate(ProductImageModel model);
        Task<BaseResult> Delete(int id);
        Task<BaseResult> Delete(int id1, int id2);

        IQueryable<ImageItem> GetImageByProduct(int id);

        IQueryable<ProductImageItem> GetProductImage(int id);
    }

    public class ProductImageService : IProductImageService
    {
        private readonly IRepository<ProductImage> _ProductImageRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Image> _imageRepository;

        public ProductImageService(IRepository<ProductImage> ProductImageRepository, IRepository<Product> productRepository, IRepository<Image> imageRepository)
        {
            _ProductImageRepository = ProductImageRepository;
            _productRepository = productRepository;
            _imageRepository = imageRepository;
        }

        public async Task<SmartTableResult<ProductImageItem>> Search(SmartTableParam param)
        {
            var query = _ProductImageRepository.Query();
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
            //param.Sort = new Sort() { Predicate = "DisplayOrder", Reverse = false };
            var gridData = query.ToSmartTableResult(param, x => x.ToItem());
            return gridData;
        }

        public IQueryable<ImageItem> GetImageByProduct(int id)
        {
            var image = _ProductImageRepository.Query().Where(x => x.ProductId == id).OrderBy(x => x.DisplayOrder).Select(x => x.Image.ToItem());
            return image;
        }

        public IQueryable<ProductImageItem> GetProductImage(int id)
        {
            var productImage = _ProductImageRepository.Query().Where(x => x.ProductId == id).Select(x => x.ToItem());
            return productImage;
        }

        #region CRUD

        public ProductImageModel Get(int id)
        {
            return id > 0 ? _ProductImageRepository.GetById(id).ToModel() : new ProductImageModel();
        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var productForDelete = _ProductImageRepository.GetById(id);
            if (productForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có thì xóa làm sao (o_ _)ﾉ彡☆";
                return result;
            }
            try
            {
                await _ProductImageRepository.DeleteAsync(productForDelete);
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
            var productForDelete = _ProductImageRepository.Query().FirstOrDefault(x => x.ProductId == id1 && x.ImageId == id2);
            if (productForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có thì xóa làm sao (o_ _)ﾉ彡☆";
                return result;
            }
            try
            {
                await _ProductImageRepository.DeleteAsync(productForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        public async Task<BaseResult> CreateOrUpdate(ProductImageModel model)
        {
            var result = new BaseResult();
            var ProductImage = model.ToProductImage();

            //Cập nhật thông tin chung của thực thể
            ProductImage = ProductImage.UpdateCommonInt();

            // kiểm tra trùng lặp trong dtb
            var query = _ProductImageRepository.Query().FirstOrDefault(
                x => (x.ProductId == ProductImage.ProductId && x.ImageId == ProductImage.ImageId));
            if (query != null)
            {
                result.Result = Result.Failed;
                result.Message = "Ảnh đã được thêm trước đó!";
                return result;
            }
            // Kiểm tra xem sp và hình ảnh có tồn tại hay không
            var isProductExist = _productRepository.Query().FirstOrDefault(
                x => x.Id == ProductImage.ProductId);
            var isImageExist = _productRepository.Query().FirstOrDefault(
                x => x.Id == ProductImage.ImageId);
            if (isProductExist == null)
            {
                result.Result = Result.Failed;
                result.Message = "Sản phẩm không tồn tại!";
                return result;
            }
            if (isImageExist == null)
            {
                result.Result = Result.Failed;
                result.Message = "Hình ảnh không tồn tại!";
                return result;
            }

            if (ProductImage.Id > 0)
            {
                //Cập nhật
                return await Update(ProductImage);
            }
            else
            {
                //Thêm mới
                return await Create(ProductImage);
            }
        }

        private async Task<BaseResult> Update(ProductImage ProductImage)
        {
            var result = new BaseResult() { Result = Result.Success };
            var ProductImageForUpdate = _ProductImageRepository.Query().FirstOrDefault(p => p.Id == ProductImage.Id);
            if (ProductImageForUpdate == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy sản phẩm liên quan yêu cầu!";
                return result;
            }
            try
            {
                ProductImageForUpdate = ProductImage.ToProductImage(ProductImageForUpdate);

                //Cập nhật thông tin chung của thực thể
                ProductImageForUpdate = ProductImageForUpdate.UpdateCommonInt();

                await _ProductImageRepository.UpdateAsync(ProductImageForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        private async Task<BaseResult> Create(ProductImage ProductImage)
        {
            var result = new CreateOrUpdateResultInt();
            int displayOrder = _ProductImageRepository.Query().Where(x => x.ProductId == ProductImage.ProductId).Count();
            ProductImage.DisplayOrder = displayOrder;
            try
            {
                await _ProductImageRepository.InsertAsync(ProductImage);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = ProductImage.Id;
            return result;
        }


        #endregion
    }
}
