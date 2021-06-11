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
    public interface IDiscountOrderService
    {
        Task<SmartTableResult<DiscountOrderItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(DiscountOrderModel model, int updateBy = 0, string updateByUserName = "");
        DiscountOrderModel Get(int id);
        Task<BaseResult> Delete(int id);
    }

    public class DiscountOrderService : IDiscountOrderService
    {
        private readonly IRepository<DiscountOrder> _discountOrderRepository;

        public DiscountOrderService(IRepository<DiscountOrder> discountOrderRepository)
        {
            _discountOrderRepository = discountOrderRepository;
        }

        public async Task<SmartTableResult<DiscountOrderItem>> Search(SmartTableParam param)
        {
            var query = _discountOrderRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.DiscountId.ToString().Contains(keyword));
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



        public IQueryable<DiscountOrder> GetAll()
        {
            return _discountOrderRepository.Query();
        }

        public DiscountOrderModel Get(int id)
        {
            return id > 0 ? _discountOrderRepository.GetById(id).ToModel() : new DiscountOrderModel();

        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var discountForOrderDelete = _discountOrderRepository.GetById(id);
            if (discountForOrderDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có discount cần xóa";
                return result;
            }
            try
            {
                await _discountOrderRepository.DeleteAsync(discountForOrderDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }



        public async Task<BaseResult> CreateOrUpdate(DiscountOrderModel model, int updateBy = 0, string updateByUserName = "")
        {
            var discountOrder = model.ToDiscountOrder();

            //Cập nhật thông tin chung của thực thể
            discountOrder = discountOrder.UpdateCommonInt(updateBy, updateByUserName);

            if (discountOrder.Id > 0)
            {
                //Cập nhật
                return await Update(discountOrder);
            }
            else
            {
                //Thêm mới
                return await Create(discountOrder);
            }
        }

        private async Task<BaseResult> Update(DiscountOrder discountOrder, int updateBy = 0, string updateByUserName = "")
        {
            var result = new BaseResult() { Result = Result.Success };
            var discountOrderForUpdate = _discountOrderRepository.Query().FirstOrDefault(p => p.Id == discountOrder.Id);
            if (discountOrderForUpdate == null || discountOrder.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy";
                return result;
            }
            try
            {
                discountOrderForUpdate = discountOrder.ToDiscountOrder(discountOrderForUpdate);

                //Cập nhật thông tin chung của thực thể
                discountOrderForUpdate = discountOrderForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await _discountOrderRepository.UpdateAsync(discountOrderForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        private async Task<BaseResult> Create(DiscountOrder discountOrder)
        {
            var result = new BaseResult();
            discountOrder.DiscountId = discountOrder.DiscountId;
            try
            {
                await _discountOrderRepository.InsertAsync(discountOrder);
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