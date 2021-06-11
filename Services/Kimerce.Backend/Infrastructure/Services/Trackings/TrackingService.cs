using Kimerce.Backend.Domain.Notification;
using Kimerce.Backend.Domain.Tracking;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Notification;
using Kimerce.Backend.Dto.Models.Notification;
using Kimerce.Backend.Dto.Models.Tracking;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Services.Trackings
{
    public interface ITrackingService
    {
        Task<SmartTableResult<Tracking>> Search(SmartTableParam param);
        Tracking GetId(int id);
        Task<BaseResult> Delete(int id);
        Task<BaseResult> CreateOrUpdate(Tracking model, int updateBy = 0, string updateByUserName = "");

    }

    public class TrackingService : ITrackingService
    {
        private readonly Repository<Tracking> repository;
        public TrackingService(Repository<Tracking> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Tìm kiếm danh sách DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<SmartTableResult<Tracking>> Search(SmartTableParam param)
        {
            var query = repository.Query();
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

        public IQueryable<Tracking> Index()
        {
            return repository.Query();
        }

        public Tracking GetId(int id)
        {
            var res = repository.Query().Where(e => e.Id == id).FirstOrDefault();
            return res;
        }
        private async Task<BaseResult> Create(Tracking product)
        {
            var result = new BaseResult();
            product.Title = product.Title.Trim();
            product.SubTitle = product.SubTitle.Trim();
            product.EntityId = product.EntityId;
            product.EnType = product.EnType;
            product.AcType = product.AcType;
            try
            {
                await repository.InsertAsync(product);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }


        private async Task<BaseResult> Update(Tracking product, int updateBy = 0, string updateByUserName = "")
        {
            var result = new BaseResult() { Result = Result.Success };
            var productForUpdate = repository.GetById(product.Id);
            if (productForUpdate == null || product.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy sản phẩm yêu cầu!";
                return result;
            }
            try
            {
                productForUpdate = product.ToTracking(productForUpdate);
                productForUpdate.Title = product.Title.Trim();
                productForUpdate.SubTitle = product.SubTitle;
                productForUpdate.EntityId = product.EntityId;
                productForUpdate.EnType = product.EnType;
                productForUpdate.AcType = product.AcType;
                //Cập nhật thông tin chung của thực thể
                productForUpdate = productForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await repository.UpdateAsync(productForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        public async Task<BaseResult> CreateOrUpdate(Tracking model, int updateBy = 0, string updateByUserName = "")
        {
            var product = model;

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

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var productForDelete = repository.Query().FirstOrDefault(p => p.Id == id);
            if (productForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có";
                return result;
            }
            try
            {
                await repository.DeleteAsync(productForDelete);
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
