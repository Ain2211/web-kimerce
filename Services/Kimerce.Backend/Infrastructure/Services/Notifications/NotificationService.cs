using Kimerce.Backend.Domain.Notification;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Notification;
using Kimerce.Backend.Dto.Models.Notification;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Services.Notifications
{
    public interface INotificationService
    {
        Task<SmartTableResult<Notification>> Search(SmartTableParam param);
        Notification GetId(int id);
        Task<BaseResult> Delete(int id);
        Task<BaseResult> CreateOrUpdate(NotificationModel model, int updateBy = 0, string updateByUserName = "");

    }

    public class NotificationService : INotificationService
    {
        private readonly Repository<Notification> _cityRepository;
        public NotificationService(Repository<Notification> repository)
        {
            _cityRepository = repository;
        }

        /// <summary>
        /// Tìm kiếm danh sách DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<SmartTableResult<Notification>> Search(SmartTableParam param)
        {
            var query = _cityRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.Title.Contains(keyword));
                }

                if (search.CreatedStart != null && search.CreatedEnd != null)
                {
                    DateTimeOffset start = search.CreatedStart;
                    DateTimeOffset end = search.CreatedEnd;
                    query = query.Where(x => (x.CreatedTime >= start) && (x.CreatedTime <= end));
                }
            }

            param.Sort = new Sort() { Predicate = "DisplayOrder", Reverse = false };
            var gridData = query.ToSmartTableResult(param, x => x);
            return gridData;
        }



        //GetCitiesNotByDcId

        #region CRUD

        public IQueryable<Notification> Index()
        {
            return _cityRepository.Query();
        }

        public Notification GetId(int id)
        {
            var res = _cityRepository.Query().Where(e => e.Id == id).FirstOrDefault();
            return res;
        }
        private async Task<BaseResult> Create(Notification product)
        {
            var result = new BaseResult();
            product.Title = product.Title.Trim();
            product.SubTitle = product.SubTitle.Trim();
            product.UpdatedTime = DateTimeOffset.Now;
            try
            {
                await _cityRepository.InsertAsync(product);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }


        private async Task<BaseResult> Update(Notification product, int updateBy = 0, string updateByUserName = "")
        {
            var result = new BaseResult() { Result = Result.Success };
            var productForUpdate = _cityRepository.Query().FirstOrDefault(p => p.Id == product.Id);
            if (productForUpdate == null || product.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy sản phẩm yêu cầu!";
                return result;
            }
            try
            {
                productForUpdate = product.ToNotification(productForUpdate);
                productForUpdate.Title = product.Title.Trim();
                productForUpdate.SubTitle = product.SubTitle;
                productForUpdate.NoticationConfigld = product.NoticationConfigld;
                productForUpdate.ReadDate = DateTimeOffset.Now;
                //Cập nhật thông tin chung của thực thể
                productForUpdate = productForUpdate.UpdateCommonLong(updateBy, updateByUserName);

                await _cityRepository.UpdateAsync(productForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        public async Task<BaseResult> CreateOrUpdate(NotificationModel model, int updateBy = 0, string updateByUserName = "")
        {
            var product = model.ToNotification();

            //Cập nhật thông tin chung của thực thể
            product = product.UpdateCommonLong(updateBy, updateByUserName);


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

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var productForDelete = _cityRepository.Query().FirstOrDefault(p => p.Id == id);
            if (productForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có";
                return result;
            }
            try
            {
                await _cityRepository.DeleteAsync(productForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }




        #endregion



    }
}
