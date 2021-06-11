using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Domain.News;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.News;
using Kimerce.Backend.Dto.Items.Products;
using Kimerce.Backend.Dto.Models.News;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Services.NewImage
{
    public interface INewsImageService
    {
        Task<SmartTableResult<NewsImageItem>> Search(SmartTableParam param);
        NewsImageModel Get(int id);
        Task<BaseResult> CreateOrUpdate(NewsImageModel model);
        Task<BaseResult> Delete(int id);
        Task<BaseResult> Delete(int id1, int id2);

        IQueryable<ImageItem> GetImageByNews(int id);

        IQueryable<NewsImageItem> GetNewsImage(int id);
    }
    public class NewsImageService : INewsImageService
    {
        private readonly IRepository<NewsImage> _newsImageRepository;
        private readonly IRepository<News> _newsRepository;
        private readonly IRepository<Image> _imageRepository;

        public NewsImageService(IRepository<NewsImage> newsImageRepository, IRepository<News> newsRepository, IRepository<Image> imageRepository)
        {
            _newsImageRepository = newsImageRepository;
            _newsRepository = newsRepository;
            _imageRepository = imageRepository;
        }

        public async Task<SmartTableResult<NewsImageItem>> Search(SmartTableParam param)
        {
            var query = _newsImageRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.NewsId.ToString().Contains(keyword));
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

        public IQueryable<ImageItem> GetImageByNews(int id)
        {
            var image = _newsImageRepository.Query().Where(x => x.NewsId == id).OrderBy(x => x.DisplayOrder).Select(x => x.Image.ToItem());
            return image;
        }

        public IQueryable<NewsImageItem> GetNewsImage(int id)
        {
            var newsImage = _newsImageRepository.Query().Where(x => x.NewsId == id).Select(x => x.ToItem());
            return newsImage;
        }

        #region CRUD

        public NewsImageModel Get(int id)
        {
            return id > 0 ? _newsImageRepository.GetById(id).ToModel() : new NewsImageModel();
        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var newsForDelete = _newsImageRepository.GetById(id);
            if (newsForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "không tìm thấy !";
                return result;
            }
            try
            {
                await _newsImageRepository.DeleteAsync(newsForDelete);
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
            var newsForDelete = _newsImageRepository.Query().FirstOrDefault(x => x.NewsId == id1 && x.ImageId == id2);
            if (newsForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy";
                return result;
            }
            try
            {
                await _newsImageRepository.DeleteAsync(newsForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        public async Task<BaseResult> CreateOrUpdate(NewsImageModel model)
        {
            var result = new BaseResult();
            var NewsImage = model.ToNewsImage();

            //Cập nhật thông tin chung của thực thể
            NewsImage = NewsImage.UpdateCommonInt();

            // kiểm tra trùng lặp trong dtb
            var query = _newsImageRepository.Query().FirstOrDefault(
                x => (x.NewsId == NewsImage.NewsId && x.ImageId == NewsImage.ImageId));
            if (query != null)
            {
                result.Result = Result.Failed;
                result.Message = "Ảnh đã được thêm trước đó!";
                return result;
            }
            // Kiểm tra xem sp và hình ảnh có tồn tại hay không
            var isNewsExist = _newsRepository.Query().FirstOrDefault(
                x => x.Id == NewsImage.NewsId);
            var isImageExist = _newsRepository.Query().FirstOrDefault(
                x => x.Id == NewsImage.ImageId);
            if (isNewsExist == null)
            {
                result.Result = Result.Failed;
                result.Message = "Blog không tồn tại!";
                return result;
            }
            if (isImageExist == null)
            {
                result.Result = Result.Failed;
                result.Message = "Hình ảnh không tồn tại!";
                return result;
            }

            if (NewsImage.Id > 0)
            {
                //Cập nhật
                return await Update(NewsImage);
            }
            else
            {
                //Thêm mới
                return await Create(NewsImage);
            }
        }

        private async Task<BaseResult> Update(NewsImage newsImage)
        {
            var result = new BaseResult() { Result = Result.Success };
            var NewsImageForUpdate = _newsImageRepository.Query().FirstOrDefault(p => p.Id == newsImage.Id);
            if (NewsImageForUpdate == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy blog liên quan yêu cầu!";
                return result;
            }
            try
            {
                NewsImageForUpdate = newsImage.ToNewsImage(NewsImageForUpdate);

                //Cập nhật thông tin chung của thực thể
                NewsImageForUpdate = NewsImageForUpdate.UpdateCommonInt();

                await _newsImageRepository.UpdateAsync(NewsImageForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        private async Task<BaseResult> Create(NewsImage newsImage)
        {
            var result = new CreateOrUpdateResultInt();
            int displayOrder = _newsImageRepository.Query().Where(x => x.NewsId == newsImage.NewsId).Count();
            newsImage.DisplayOrder = displayOrder;
            try
            {
                await _newsImageRepository.InsertAsync(newsImage);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = newsImage.Id;
            return result;
        }


        #endregion
    }
}
