using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Models.Images;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Services.Images
{
    public interface IImageService
    {
        /// <summary>
        /// Tìm kiếm Image
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<SmartTableResult<ImageItem>> Search(SmartTableParam param);

        /// <summary>
        /// Tạo / cập nhật Image
        /// </summary>
        /// <param name="model"></param>
        /// <param name="updateBy"></param>
        /// <param name="updateByUserName"></param>
        /// <returns></returns>
        Task<BaseResult> CreateOrUpdate(ImageModel model, int updateBy = 0, string updateByUserName = "");

        /// <summary>
        /// Xóa Image
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult> Delete(int id);

        /// <summary>
        /// Lấy thông tin image
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Image Get(int id);


    }
    public class ImageService: IImageService
    {

        private readonly IRepository<Image> _Repository;

        public ImageService(IRepository<Image> Repository)
        {
            this._Repository = Repository;
        }

        public IQueryable<Image> GetAll()
        {
            return _Repository.Query();
        }

        public Image Get(int id)
        {
            return _Repository.GetById(id);
        }
        public async Task<ActionResult<IEnumerable<Image>>> Index()
        {
            return await _Repository.Query().ToListAsync();

        }

        public async Task<SmartTableResult<ImageItem>> Search(SmartTableParam param)
        {
            var query = _Repository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.name.Contains(keyword));
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

        public async Task<BaseResult> Update(Image image, int updateBy = 0, string updateByUserName = "")
        {

            var rs = new BaseResult() { Result = Result.Success };
            var orderForUpdate = _Repository.Query().FirstOrDefault(p => p.Id == image.Id);
            if (orderForUpdate != null)
            {

                try
                {
                    orderForUpdate = image.ToImage(orderForUpdate);

                   // orderForUpdate = orderForUpdate.UpdateCommonInt(updateBy, updateByUserName);
                    await _Repository.UpdateAsync(orderForUpdate);
                    _Repository.SaveChange();
                }
                catch (Exception ex)
                {
                    rs.Result = Result.SystemError;
                    rs.Message = ex.ToString();
                }
            }
            else
            {
                rs.Message = "Không tìm thấy Image cần sửa";
                rs.Result = Result.Failed;
            }
            return rs;
        }

        public async Task<BaseResult> Create(Image image)
        {
            var rs = new BaseResult() { Result = Result.Success };



            try
            {
                await _Repository.InsertAsync(image);
            }
            catch (Exception ex)
            {
                rs.Result = Result.SystemError;
                rs.Message = ex.ToString();
            }
            return rs;
        }

        public async Task<BaseResult> CreateOrUpdate(ImageModel model, int updateBy = 0, string updateByUserName = "")
        {
            var image = model.ToImage();
            image.CreatedTime = DateTime.Now;
            if (image.Id > 0)
            {
                //Cập nhật
                return await Update(image);
            }
            else
            {
                //Thêm mới
                return await Create(image);
            }

        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var productForDelete = _Repository.GetById(id);
            if (productForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có ";
                return result;
            }
            try
            {
                await _Repository.DeleteAsync(productForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }
    }
}
