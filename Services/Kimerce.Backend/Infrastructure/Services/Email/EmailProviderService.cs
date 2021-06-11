using Kimerce.Backend.Domain.Email;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Email;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Services.EmailService
{
    public interface IEmailProviderService
    {
        SmartTableResult<EmailProviderItem> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(EmailProviderItem model);
        IQueryable<EmailAccountItem> GetAccounts(long Id);
        EmailProviderItem Get(long id);
        Task<BaseResult> Delete(long id);
    }
    public class EmailProviderService : IEmailProviderService
    {
        private readonly IRepository<EmailProvider> _emailProviderRepository;
        private readonly IRepository<EmailAccount> _emailAccountRepository;
        private readonly IEmailAccountService _emailAccountService;
        public EmailProviderService(IRepository<EmailProvider> emailProviderRepository, IRepository<EmailAccount> emailAccountRepository, IEmailAccountService emailAccountService)
        {
            _emailProviderRepository = emailProviderRepository;
            _emailAccountRepository = emailAccountRepository;
            _emailAccountService = emailAccountService;
        }

        /// <summary>
        /// Tìm kiếm danh sách DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SmartTableResult<EmailProviderItem> Search(SmartTableParam param)
        {
            var query = _emailProviderRepository.Query().Include(p => p.Accounts).AsNoTracking();

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

        public IQueryable<EmailAccountItem> GetAccounts(long Id)
        {
            return _emailAccountRepository.Query().Where(a => a.ProviderId == Id).Select(a => a.ToItem());
        }
        public EmailProviderItem Get(long id)
        {
            var query = _emailProviderRepository.Query().Include(p => p.Accounts).AsNoTracking().FirstOrDefault(p => p.Id == id).ToItem();
            return query;
        }
        //GetCitiesNotByDcId

        #region CRUD
        private async Task<BaseResult> Create(EmailProvider emailProvider)
        {
            var rs = new BaseResult() { Result = Result.Success };
            emailProvider.Name = emailProvider.Name.Trim();
            emailProvider.CreatedTime = DateTimeOffset.Now;

            var exists = _emailProviderRepository.Query().Any(ep => ep.Name == emailProvider.Name);
            if (exists)
            {
                rs.Result = Result.Failed;
                rs.Message = "Nhà cung cấp đã tồn tại";
                return rs;
            }
            await _emailProviderRepository.InsertAsync(emailProvider);
            try
            {
                _emailProviderRepository.SaveChange();
            }
            catch (Exception ex)
            {
                rs.Result = Result.SystemError;
                rs.Message = ex.ToString();
            }
            return rs;
        }

        private async Task<BaseResult> Update(EmailProvider emailProviderItem)
        {

            var rs = new BaseResult() { Result = Result.Success };
            var emailProvider = _emailProviderRepository.Query().Where(ep => !ep.IsDeleted).FirstOrDefault(ep => ep.Id == emailProviderItem.Id);
            if (emailProvider != null)
            {
                emailProvider.Name = emailProviderItem.Name.Trim();
                emailProvider.LimitNumber = emailProviderItem.LimitNumber;
                emailProvider.Port = emailProviderItem.Port;
                emailProvider.Host = emailProviderItem.Host;
                emailProvider.UpdatedTime = DateTimeOffset.Now;

                await _emailProviderRepository.UpdateAsync(emailProvider);
                try
                {
                    _emailProviderRepository.SaveChange();
                }
                catch (Exception ex)
                {
                    rs.Result = Result.SystemError;
                    rs.Message = ex.ToString();
                }
            }
            else
            {
                rs.Message = "Không tìm thấy nhà cung cấp cần sửa";
                rs.Result = Result.Failed;
            }
            return rs;
        }

        public async Task<BaseResult> CreateOrUpdate(EmailProviderItem emailProviderItem)
        {
            var emailProvider = emailProviderItem.ToEmailProvider();
            var rs = new BaseResult() { Result = Result.Success };
            return emailProvider.Id <= 0 ? await Create(emailProvider) : await Update(emailProvider);

        }

        public async Task<BaseResult> Delete(long emailProviderId)
        {
            var rs = new BaseResult() { Result = Result.Success };
            if (emailProviderId > 0)
            {
                var emailProvider = _emailProviderRepository.Query().FirstOrDefault(ep => ep.Id == emailProviderId);
                if (emailProvider != null)
                {
                    var emailAccounts = _emailAccountRepository.Query().Where(a => a.ProviderId == emailProviderId).ToList();
                    foreach (var account in emailAccounts)
                    {
                        await _emailAccountService.Delete(account.Id);
                    }
                    emailProvider.IsDeleted = true;
                    await _emailProviderRepository.UpdateAsync(emailProvider);
                    try
                    {
                        _emailProviderRepository.SaveChange();
                    }
                    catch (Exception ex)
                    {
                        rs.Result = Result.SystemError;
                        rs.Message = ex.ToString();
                    }
                }
                else
                {
                    rs.Message = "Không tìm thấy nhà cung cấp yêu cầu!";
                    rs.Result = Result.Failed;
                }

            }
            else
            {
                rs.Message = "Mã nhà cung cấp không hợp lệ!";
                rs.Result = Result.Failed;
            }
            return rs;
        }
        #endregion
    }
}
