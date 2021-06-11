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
    public interface IEmailService
    {
        SmartTableResult<EmailItem> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(EmailItem model);
        EmailItem Get(long id);
        Task<BaseResult> Delete(long id);
    }
    public class EmailService : IEmailService
    {
        private readonly IRepository<Email> _emailRepository;
        private readonly IRepository<EmailAccount> _emailAccountRepository;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        public EmailService(IRepository<Email> emailRepository, IRepository<EmailAccount> emailAccountRepository, IRepository<EmailTemplate> emailTemplateRepository)
        {
            _emailRepository = emailRepository;
            _emailAccountRepository = emailAccountRepository;
            _emailTemplateRepository = emailTemplateRepository;
        }

        /// <summary>
        /// Tìm kiếm danh sách DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SmartTableResult<EmailItem> Search(SmartTableParam param)
        {
            var query = _emailRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.Subject.Contains(keyword));
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
        public EmailItem Get(long id)
        {
            var query = _emailRepository.Query().Where(e => e.Id == id)
                .Include(e => e.Template)
                .Include(e => e.Account)
                .ThenInclude(a => a.Provider)
                .AsNoTracking()
                .FirstOrDefault().ToItem();

            return query;
        }
        //GetCitiesNotByDcId

        #region CRUD
        private async Task<BaseResult> Create(Email email)
        {
            var rs = new BaseResult() { Result = Result.Success };
            var emailAccount = _emailAccountRepository.Query().Where(a => !a.IsDeleted)
                .Include(a => a.Emails)
                    .FirstOrDefault(a => a.Id == email.AccountId);
            if (emailAccount == null)
            {
                rs.Result = Result.Failed;
                rs.Message = "Không tồn tại tài khoản";
                return rs;
            }
            email.Account = emailAccount;

            var emailTemplate = _emailTemplateRepository.Query().Where(t => !t.IsDeleted)
                .Include(a => a.Emails)
                    .FirstOrDefault(a => a.Id == email.TemplateId);
            if (emailTemplate == null)
            {
                rs.Result = Result.Failed;
                rs.Message = "Không tồn tại mẫu";
                return rs;
            }
            email.Template = emailTemplate;
            email.From = email.From.Trim();
            email.To = email.To.Trim();
            email.CreatedTime = DateTimeOffset.Now;
            await _emailRepository.InsertAsync(email);
            try
            {
                _emailRepository.SaveChange();
            }
            catch (Exception ex)
            {
                rs.Result = Result.SystemError;
                rs.Message = ex.ToString();
            }
            return rs;
        }

        private async Task<BaseResult> Update(Email newEmail)
        {

            var rs = new BaseResult() { Result = Result.Success };
            var email = _emailRepository.Query().Where(e => !e.IsDeleted).FirstOrDefault(e => e.Id == newEmail.Id);
            var emailAccount = _emailAccountRepository.Query().Where(a => !a.IsDeleted)
                .FirstOrDefault(p => p.Id == email.AccountId);
            if (emailAccount == null)
            {
                rs.Result = Result.Failed;
                rs.Message = "Không tồn tại nhà cung cấp";
                return rs;
            }
            email.Account = emailAccount;

            var emailTemplate = _emailTemplateRepository.Query().Where(t => !t.IsDeleted)
                .Include(a => a.Emails)
                    .FirstOrDefault(a => a.Id == email.TemplateId);
            if (emailTemplate == null)
            {
                rs.Result = Result.Failed;
                rs.Message = "Không tồn tại mẫu";
                return rs;
            }
            email.Template = emailTemplate;

            if (email != null)
            {
                email.From = newEmail.From.Trim();
                email.To = newEmail.To.Trim();
                email.Body = newEmail.Body;
                email.Subject = newEmail.Subject;
                email.UpdatedTime = DateTimeOffset.Now;

                await _emailRepository.UpdateAsync(email);
                try
                {
                    _emailRepository.SaveChange();
                }
                catch (Exception ex)
                {
                    rs.Result = Result.SystemError;
                    rs.Message = ex.ToString();
                }
            }
            else
            {
                rs.Message = "Không tìm thấy email cần sửa";
                rs.Result = Result.Failed;
            }
            return rs;
        }

        public async Task<BaseResult> CreateOrUpdate(EmailItem newEmail)
        {
            var email = newEmail.ToEmail();
            var rs = new BaseResult() { Result = Result.Success };
            return email.Id <= 0 ? await Create(email) : await Update(email);

        }

        public async Task<BaseResult> Delete(long emailId)
        {
            var rs = new BaseResult() { Result = Result.Success };
            if (emailId > 0)
            {
                var email = _emailRepository.Query().FirstOrDefault(ea => ea.Id == emailId);
                if (email != null)
                {
                    email.IsDeleted = true;
                    await _emailRepository.UpdateAsync(email);
                    try
                    {
                        _emailRepository.SaveChange();
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
