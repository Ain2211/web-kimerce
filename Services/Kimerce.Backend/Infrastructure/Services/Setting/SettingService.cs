using Kimerce.Backend.Domain.Setting;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Setting;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Services
{
    public interface ISettingService
    {
        SmartTableResult<SettingItem> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(SettingItem model);
        SettingItem Get(int id);
        Task<BaseResult> Delete(int id);
    }
    public class SettingService : ISettingService
    {

        private readonly IRepository<Setting> _settingRepository;
        public SettingService(IRepository<Setting> settingRepository)
        {
            _settingRepository = settingRepository;
        }

        /// <summary>
        /// Tìm kiếm danh sách DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SmartTableResult<SettingItem> Search(SmartTableParam param)
        {
            var query = _settingRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.Name.Contains(keyword));
                }
                if (search.CreatedStart != null)
                {
                    DateTime createStart = DateTime.Parse(search.CreatedStart.ToString());
                    DateTime startOfDay = createStart.StartOfDay();
                    query = query.Where(x => x.CreatedTime >= startOfDay);
                }

                if (search.CreatedEnd != null)
                {
                    DateTime createEnd = DateTime.Parse(search.CreatedEnd.ToString());
                    DateTime endOfDay = createEnd.EndOfDay();
                    query = query.Where(x => x.CreatedTime <= endOfDay);
                }
            }
            param.Sort = new Sort() { Predicate = "", Reverse = false };
            var gridData = query.ToSmartTableResult(param, x => x.ToItem());
            return gridData;
        }

        public IQueryable<SettingItem> GetAll()
        {
            return _settingRepository.Query().Select(x => x.ToItem());
        }
        public SettingItem Get(int id)
        {
            var query = _settingRepository.GetById(id).ToItem();

            return query;
        }
        //GetCitiesNotByDcId

        #region CRUD
        private async Task<BaseResult> Create(Setting setting)
        {
            var rs = new BaseResult() { Result = Result.Success };
            setting.CreatedTime = DateTimeOffset.Now;
            setting.Name = setting.Name.Trim();
            setting.SystemName = setting.SystemName.Trim();
            var exists = _settingRepository.Query().Any(s => s.Name == setting.Name || s.SystemName == setting.SystemName);
            if (exists)
            {
                rs.Result = Result.Failed;
                rs.Message = "Cấu hình đã tồn tại";
                return rs;
            }
            await _settingRepository.InsertAsync(setting);
            try
            {
                _settingRepository.SaveChange();
            }
            catch (Exception ex)
            {
                rs.Result = Result.SystemError;
                rs.Message = ex.ToString();
            }
            return rs;
        }

        private async Task<BaseResult> Update(Setting settingItem)
        {

            var rs = new BaseResult() { Result = Result.Success };
            var setting = _settingRepository.Query().Where(s => !s.IsDeleted).FirstOrDefault(s => s.Id == settingItem.Id);
            if (setting != null)
            {
                var exists = _settingRepository.Query().Any(s => !s.IsDeleted && s.Name == settingItem.Name && s.Id != setting.Id);
                if (exists)
                {
                    rs.Result = Result.Failed;
                    rs.Message = "Cấu hình đã tồn tại!";
                    return rs;
                }

                setting.Name = settingItem.Name.Trim();
                setting.SystemName = settingItem.SystemName.Trim();
                setting.Value = settingItem.Value;
                setting.UpdatedTime = DateTimeOffset.Now;

                await _settingRepository.UpdateAsync(setting);
                try
                {
                    _settingRepository.SaveChange();
                }
                catch (Exception ex)
                {
                    rs.Result = Result.SystemError;
                    rs.Message = ex.ToString();
                }
            }
            else
            {
                rs.Message = "Không tìm thấy cấu hình cần sửa";
                rs.Result = Result.Failed;
            }
            return rs;
        }

        public async Task<BaseResult> CreateOrUpdate(SettingItem settingItem)
        {
            var setting = settingItem.ToSetting();
            var rs = new BaseResult() { Result = Result.Success };
            return setting.Id <= 0 ? await Create(setting) : await Update(setting);

        }

        public async Task<BaseResult> Delete(int settingId)
        {
            var rs = new BaseResult() { Result = Result.Success };
            if (settingId > 0)
            {
                var setting = _settingRepository.Query().FirstOrDefault(s => s.Id == settingId);
                if (setting != null)
                {
                    await _settingRepository.DeleteAsync(setting);
                    try
                    {
                        _settingRepository.SaveChange();
                    }
                    catch (Exception ex)
                    {
                        rs.Result = Result.SystemError;
                        rs.Message = ex.ToString();
                    }
                }
                else
                {
                    rs.Message = "Không tìm thấy cấu hình yêu cầu!";
                    rs.Result = Result.Failed;
                }

            }
            else
            {
                rs.Message = "Mã cấu hình không hợp lệ!";
                rs.Result = Result.Failed;
            }
            return rs;
        }
        #endregion



    }
}
