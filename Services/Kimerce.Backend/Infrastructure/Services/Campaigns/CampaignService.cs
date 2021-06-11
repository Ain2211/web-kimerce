using Kimerce.Backend.Domain.Campaigns;
using Kimerce.Backend.Domain.Email;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Campaigns;
using Kimerce.Backend.Dto.Items.Email;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Services.Campaigns
{
    public interface ICampaignService
    {
        /// <summary>
        /// Tìm kiếm chiến dịch
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        SmartTableResult<CampaignItem> Search(SmartTableParam param);

        /// <summary>
        /// Lấy thông tin chiến dịch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CampaignItem Get(int id);

        /// <summary>
        /// Tạo / cập nhật chiến dịch
        /// </summary>
        /// <param name="campaignItem"></param>
        /// <returns></returns>
        Task<BaseResult> CreateOrUpdate(CampaignItem campaignItem);

        /// <summary>
        /// Xóa chiến dịch
        /// </summary>
        /// <param name="campaignItemID"></param>
        /// <returns></returns>
        Task<BaseResult> Delete(int campaignId);
    }
    public class CampaignService : ICampaignService
    {
        private readonly IRepository<Campaign> _campaignRepository;
        public CampaignService(IRepository<Campaign> campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        /// <summary>
        /// Tìm kiếm danh sách DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SmartTableResult<CampaignItem> Search(SmartTableParam param)
        {
            var query = _campaignRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.Subject.Contains(keyword));
                }
            }
            param.Sort = new Sort() { Predicate = "", Reverse = false };
            var gridData = query.ToSmartTableResult(param, x => x.ToItem());
            return gridData;
        }

        public IQueryable<CampaignItem> GetAll()
        {
            return _campaignRepository.Query().Select(x => x.ToItem());
        }
        public CampaignItem Get(int id)
        {
            var query = _campaignRepository.Query().Where(e => e.Id == id).FirstOrDefault().ToItem();

            return query;
        }
        //GetCitiesNotByDcId

        #region CRUD
        private async Task<BaseResult> Create(Campaign campaign)
        {
            var rs = new BaseResult() { Result = Result.Success };
            campaign.Subject = campaign.Subject.Trim();
            campaign.CreatedTime = DateTimeOffset.Now;

            var exists = _campaignRepository.Query().Any(ea => ea.Subject == campaign.Subject);
            if (exists)
            {
                rs.Result = Result.Failed;
                rs.Message = "Chiến dịch đã tồn tại";
                return rs;
            }
            await _campaignRepository.InsertAsync(campaign);
            try
            {
                _campaignRepository.SaveChange();
            }
            catch (Exception ex)
            {
                rs.Result = Result.SystemError;
                rs.Message = ex.ToString();
            }
            return rs;
        }

        private async Task<BaseResult> Update(Campaign campaignItem)
        {

            var rs = new BaseResult() { Result = Result.Success };
            var campaign = _campaignRepository.Query().Where(ea => !ea.IsDeleted).FirstOrDefault(ea => ea.Id == campaignItem.Id);
            if (campaign != null)
            {
                campaign.Subject = campaignItem.Subject.Trim();
                campaign.Body = campaignItem.Body;
                campaign.Budget = campaignItem.Budget;
                campaign.Status = campaignItem.Status;
                campaign.StartDate = campaignItem.StartDate;
                campaign.EndDate = campaignItem.EndDate;
                campaign.UpdatedTime = DateTimeOffset.Now;

                await _campaignRepository.UpdateAsync(campaign);
                try
                {
                    _campaignRepository.SaveChange();
                }
                catch (Exception ex)
                {
                    rs.Result = Result.SystemError;
                    rs.Message = ex.ToString();
                }
            }
            else
            {
                rs.Message = "Không tìm thấy chiến dịch cần sửa";
                rs.Result = Result.Failed;
            }
            return rs;
        }

        public async Task<BaseResult> CreateOrUpdate(CampaignItem campaignItem)
        {
            var campaign = campaignItem.ToCampaign();
            var rs = new BaseResult() { Result = Result.Success };
            return campaign.Id <= 0 ? await Create(campaign) : await Update(campaign);

        }

        public async Task<BaseResult> Delete(int campaignId)
        {
            var rs = new BaseResult() { Result = Result.Success };
            if (campaignId > 0)
            {
                var campaign = _campaignRepository.Query().FirstOrDefault(ea => ea.Id == campaignId);
                if (campaign != null)
                {
                    campaign.IsDeleted = true;
                    await _campaignRepository.UpdateAsync(campaign);
                    try
                    {
                        _campaignRepository.SaveChange();
                    }
                    catch (Exception ex)
                    {
                        rs.Result = Result.SystemError;
                        rs.Message = ex.ToString();
                    }
                }
                else
                {
                    rs.Message = "Không tìm thấy chiến dịch yêu cầu!";
                    rs.Result = Result.Failed;
                }

            }
            else
            {
                rs.Message = "Mã chiến dịch không hợp lệ!";
                rs.Result = Result.Failed;
            }
            return rs;
        }
        #endregion
    }
}
