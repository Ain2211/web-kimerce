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
    public interface ICampaignOrderService
    {
        Task<SmartTableResult<CampaignOrderItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(CampaignOrderModel model, int updateBy = 0, string updateByUserName = "");
        CampaignOrderModel Get(int id);
        Task<BaseResult> Delete(int id);
    }

    public class CampaignOrderService : ICampaignOrderService
    {
        private readonly IRepository<CampaignOrder> _campaignOrderRepository;

        public CampaignOrderService(IRepository<CampaignOrder> campaignOrderRepository)
        {
            _campaignOrderRepository = campaignOrderRepository;
        }

        public async Task<SmartTableResult<CampaignOrderItem>> Search(SmartTableParam param)
        {
            var query = _campaignOrderRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.CampaignId.ToString() == (keyword));
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



        public IQueryable<CampaignOrder> GetAll()
        {
            return _campaignOrderRepository.Query();
        }

        public CampaignOrderModel Get(int id)
        {
            return id > 0 ? _campaignOrderRepository.GetById(id).ToModel() : new CampaignOrderModel();

        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var campaignForOrderDelete = _campaignOrderRepository.GetById(id);
            if (campaignForOrderDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có campaign cần xóa";
                return result;
            }
            try
            {
                await _campaignOrderRepository.DeleteAsync(campaignForOrderDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }



        public async Task<BaseResult> CreateOrUpdate(CampaignOrderModel model, int updateBy = 0, string updateByUserName = "")
        {
            var campaignOrder = model.ToCampaignOrder();

            //Cập nhật thông tin chung của thực thể
            campaignOrder = campaignOrder.UpdateCommonInt(updateBy, updateByUserName);

            if (campaignOrder.Id > 0)
            {
                //Cập nhật
                return await Update(campaignOrder);
            }
            else
            {
                //Thêm mới
                return await Create(campaignOrder);
            }
        }

        private async Task<BaseResult> Update(CampaignOrder campaignOrder, int updateBy = 0, string updateByUserName = "")
        {
            var result = new BaseResult() { Result = Result.Success };
            var campaignOrderForUpdate = _campaignOrderRepository.Query().FirstOrDefault(p => p.Id == campaignOrder.Id);
            if (campaignOrderForUpdate == null || campaignOrder.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy";
                return result;
            }
            try
            {
                campaignOrderForUpdate = campaignOrder.ToCampaignOrder(campaignOrderForUpdate);

                //Cập nhật thông tin chung của thực thể
                campaignOrderForUpdate = campaignOrderForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await _campaignOrderRepository.UpdateAsync(campaignOrderForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        private async Task<BaseResult> Create(CampaignOrder campaignOrder)
        {
            var result = new BaseResult();
            campaignOrder.CampaignId = campaignOrder.CampaignId;
            try
            {
                await _campaignOrderRepository.InsertAsync(campaignOrder);
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