using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Campaigns;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Campaigns;
using Kimerce.Backend.Dto.Models.Campaigns;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;

namespace Kimerce.Backend.Infrastructure.Services.Campaigns
{
    public interface IDiscountService
    {
        Task<SmartTableResult<DiscountItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(DiscountModel model, int updateBy = 0, string updateByUserName = "");
        DiscountModel Get(int id);
        Task<BaseResult> Delete(int id);
    }

    public class DiscountService : IDiscountService
    {
        private readonly IRepository<Discount> _discountRepository;

        public DiscountService(IRepository<Discount> discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<SmartTableResult<DiscountItem>> Search(SmartTableParam param)
        {
            var query = _discountRepository.Query();
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


        #region CRUD



        public IQueryable<Discount> GetAll()
        {
            return _discountRepository.Query();
        }

        public DiscountModel Get(int id)
        {
            return id > 0 ? _discountRepository.GetById(id).ToModel() : new DiscountModel();

        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var discountForDelete = _discountRepository.GetById(id);
            if (discountForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có discount cần xóa";
                return result;
            }
            try
            {
                await _discountRepository.DeleteAsync(discountForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }



        public async Task<BaseResult> CreateOrUpdate(DiscountModel model, int updateBy = 0, string updateByUserName = "")
        {
            var discount = model.ToDiscount();

            //Cập nhật thông tin chung của thực thể
            discount = discount.UpdateCommonInt(updateBy, updateByUserName);

            if (discount.Id > 0)
            {
                //Cập nhật
                return await Update(discount);
            }
            else
            {
                //Thêm mới
                return await Create(discount);
            }
        }

        private async Task<BaseResult> Update(Discount discount, int updateBy = 0, string updateByUserName = "")
        {
            var result = new BaseResult() { Result = Result.Success };
            var discountForUpdate = _discountRepository.Query().FirstOrDefault(p => p.Id == discount.Id);
            if (discountForUpdate == null || discount.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy";
                return result;
            }
            try
            {
                discountForUpdate = discount.ToDiscount(discountForUpdate);

                //Cập nhật thông tin chung của thực thể
                discountForUpdate = discountForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await _discountRepository.UpdateAsync(discountForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        private async Task<BaseResult> Create(Discount discount)
        {
            var result = new BaseResult();
            discount.Name = discount.Name.Trim();
            try
            {
                await _discountRepository.InsertAsync(discount);
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