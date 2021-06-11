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
    public interface IEmailAccountService
    {
        SmartTableResult<EmailAccountItem> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(EmailAccountItem model);
        IQueryable<EmailItem> GetEmails(long id);
        EmailAccountItem Get(long id);
        Task<BaseResult> Delete(long id);
    }
    public class EmailAccountService : IEmailAccountService
    {
        private readonly IRepository<EmailAccount> _emailAccountRepository;
        private readonly IRepository<EmailProvider> _emailProviderRepository;
        private readonly IRepository<Email> _emailRepository;
        private readonly IEmailService _emailService;
        public EmailAccountService(IRepository<EmailAccount> emailAccountRepository, IRepository<EmailProvider> emailProviderRepository, IRepository<Email> emailRepository, IEmailService emailService)
        {
            _emailAccountRepository = emailAccountRepository;
            _emailProviderRepository = emailProviderRepository;
            _emailRepository = emailRepository;
            _emailService = emailService;
        }

        /// <summary>
        /// Tìm kiếm danh sách DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SmartTableResult<EmailAccountItem> Search(SmartTableParam param)
        {
            var query = _emailAccountRepository.Query().Include(a => a.Provider).AsNoTracking();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.UserName.Contains(keyword));
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

        public IQueryable<EmailItem> GetEmails(long Id)
        {
            return _emailRepository.Query().Where(a => a.AccountId == Id).Select(a => a.ToItem());
        }
        public EmailAccountItem Get(long id)
        {
            var query = _emailAccountRepository.Query().Include(a => a.Provider).Where(e => e.Id == id).FirstOrDefault().ToItem();
            
            return query;
        }
        //GetCitiesNotByDcId

        #region CRUD
        private async Task<BaseResult> Create(EmailAccount emailAccount)
        {
            var rs = new BaseResult() { Result = Result.Success };

            var emailProvider = _emailProviderRepository.Query()
                .Include(p => p.Accounts)
                .FirstOrDefault(p => p.Id == emailAccount.ProviderId);
            if(emailProvider == null)
            {
                rs.Result = Result.Failed;
                rs.Message = "Không tồn tại nhà cung cấp";
                return rs;
            }
            emailAccount.Provider = emailProvider;
            emailAccount.UserName = emailAccount.UserName.Trim();
            emailAccount.CreatedTime = DateTimeOffset.Now;
            System.Diagnostics.Debug.WriteLine(emailAccount.ProviderId);

            var exists = _emailAccountRepository.Query().Any(ea => ea.Email == emailAccount.Email);
            if (exists)
            {
                rs.Result = Result.Failed;
                rs.Message = "Tài khoản đã tồn tại";
                return rs;
            }
            await _emailAccountRepository.InsertAsync(emailAccount);
            try
            {
                _emailAccountRepository.SaveChange();
            }
            catch (Exception ex)
            {
                rs.Result = Result.SystemError;
                rs.Message = ex.ToString();
            }
            return rs;
        }

        private async Task<BaseResult> Update(EmailAccount emailAccount)
        {

            var rs = new BaseResult() { Result = Result.Success };
            var emailProvider = _emailProviderRepository.Query()
                .FirstOrDefault(p => p.Id == emailAccount.ProviderId);
            if (emailProvider == null)
            {
                rs.Result = Result.Failed;
                rs.Message = "Không tồn tại nhà cung cấp";
                return rs;
            }
            emailAccount.Provider = emailProvider;
            if (emailAccount != null)
            {
                emailAccount.UserName = emailAccount.UserName.Trim();
                emailAccount.Password = emailAccount.Password;
                emailAccount.Email = emailAccount.Email;
                emailAccount.DisplayName = emailAccount.DisplayName;
                emailAccount.UpdatedTime = DateTimeOffset.Now;

                await _emailAccountRepository.UpdateAsync(emailAccount);
                try
                {
                    _emailAccountRepository.SaveChange();
                }
                catch (Exception ex)
                {
                    rs.Result = Result.SystemError;
                    rs.Message = ex.ToString();
                }
            }
            else
            {
                rs.Message = "Không tìm thấy tài khoản cần sửa";
                rs.Result = Result.Failed;
            }
            return rs;
        }

        public async Task<BaseResult> CreateOrUpdate(EmailAccountItem emailAccountItem)
        {
            var emailAccount = emailAccountItem.ToEmailAccount();
            var rs = new BaseResult() { Result = Result.Success };
            return emailAccount.Id <= 0 ? await Create(emailAccount) : await Update(emailAccount);

        }

        public async Task<BaseResult> Delete(long emailAccountId)
        {
            var rs = new BaseResult() { Result = Result.Success };
            if (emailAccountId > 0)
            {
                var emailAccount = _emailAccountRepository.Query().FirstOrDefault(ea => ea.Id == emailAccountId);
                if (emailAccount != null)
                {
                    var emails = _emailRepository.Query().Where(a => a.AccountId == emailAccountId).ToList();
                    foreach (var email in emails)
                    {
                        await _emailService.Delete(email.Id);
                    }
                    emailAccount.IsDeleted = true;
                    await _emailAccountRepository.UpdateAsync(emailAccount);
                    try
                    {
                        _emailAccountRepository.SaveChange();
                    }
                    catch (Exception ex)
                    {
                        rs.Result = Result.SystemError;
                        rs.Message = ex.ToString();
                    }
                }
                else
                {
                    rs.Message = "Không tìm thấy tài khoản yêu cầu!";
                    rs.Result = Result.Failed;
                }

            }
            else
            {
                rs.Message = "Mã tài khoản không hợp lệ!";
                rs.Result = Result.Failed;
            }
            return rs;
        }
        #endregion
    }
}
