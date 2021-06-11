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
    public interface ICampaignProductService
    {
        Task<SmartTableResult<CampaignProductItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(CampaignProductModel model, int updateBy = 0, string updateByUserName = "");
        CampaignProductModel Get(int id);
        Task<BaseResult> Delete(int id);
    }

    public class CampaignProductService : ICampaignProductService
    {
        private readonly IRepository<CampaignProduct> _campaignProductRepository;

        public CampaignProductService(IRepository<CampaignProduct> campaignProductRepository)
        {
            _campaignProductRepository = campaignProductRepository;
        }

        public async Task<SmartTableResult<CampaignProductItem>> Search(SmartTableParam param)
        {
            var query = _campaignProductRepository.Query();
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



        public IQueryable<CampaignProduct> GetAll()
        {
            return _campaignProductRepository.Query();
        }

        public CampaignProductModel Get(int id)
        {
            return id > 0 ? _campaignProductRepository.GetById(id).ToModel() : new CampaignProductModel();

        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var campaignForOrderDelete = _campaignProductRepository.GetById(id);
            if (campaignForOrderDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có campaign cần xóa";
                return result;
            }
            try
            {
                await _campaignProductRepository.DeleteAsync(campaignForOrderDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }



        public async Task<BaseResult> CreateOrUpdate(CampaignProductModel model, int updateBy = 0, string updateByUserName = "")
        {
            var campaignProduct = model.ToCampaignProduct();

            //Cập nhật thông tin chung của thực thể
            campaignProduct = campaignProduct.UpdateCommonInt(updateBy, updateByUserName);

            if (campaignProduct.Id > 0)
            {
                //Cập nhật
                return await Update(campaignProduct);
            }
            else
            {
                //Thêm mới
                return await Create(campaignProduct);
            }
        }

        private async Task<BaseResult> Update(CampaignProduct campaignProduct, int updateBy = 0, string updateByUserName = "")
        {
            var result = new BaseResult() { Result = Result.Success };
            var campaignProductForUpdate = _campaignProductRepository.Query().FirstOrDefault(p => p.Id == campaignProduct.Id);
            if (campaignProductForUpdate == null || campaignProduct.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy";
                return result;
            }
            try
            {
                campaignProductForUpdate = campaignProduct.ToCampaignProduct(campaignProductForUpdate);

                //Cập nhật thông tin chung của thực thể
                campaignProductForUpdate = campaignProductForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await _campaignProductRepository.UpdateAsync(campaignProductForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        private async Task<BaseResult> Create(CampaignProduct campaignProduct)
        {
            var result = new BaseResult();
            campaignProduct.CampaignId = campaignProduct.CampaignId;
            try
            {
                await _campaignProductRepository.InsertAsync(campaignProduct);
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