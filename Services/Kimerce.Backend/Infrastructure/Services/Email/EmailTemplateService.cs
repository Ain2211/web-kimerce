using Kimerce.Backend.Domain.Email;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Email;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Services.EmailService
{
    public interface IEmailTemplateService
    {
        SmartTableResult<EmailTemplateItem> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(EmailTemplateItem model);
        IQueryable<EmailItem> GetEmails(long id);
        EmailTemplateItem Get(long id);
        Task<BaseResult> Delete(long id);
    }
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly IRepository<Email> _emailRepository;
        private readonly IEmailService _emailService;
        public EmailTemplateService(IRepository<EmailTemplate> emailTemplateRepository, IRepository<Email> emailRepository, IEmailService emailService)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _emailRepository = emailRepository;
            _emailService = emailService;
        }

        /// <summary>
        /// Tìm kiếm danh sách DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SmartTableResult<EmailTemplateItem> Search(SmartTableParam param)
        {
            var query = _emailTemplateRepository.Query();
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

        public IQueryable<EmailItem> GetEmails(long Id)
        {
            return _emailRepository.Query().Where(e => e.TemplateId == Id).Select(a => a.ToItem());
        }
        public EmailTemplateItem Get(long id)
        {
            var query = _emailTemplateRepository.Query().Where(e => e.Id == id).FirstOrDefault().ToItem();

            return query;
        }
        //GetCitiesNotByDcId

        #region CRUD
        private async Task<BaseResult> Create(EmailTemplate emailTemplate)
        {
            var rs = new BaseResult() { Result = Result.Success };
            emailTemplate.Name = emailTemplate.Name.Trim();
            emailTemplate.CreatedTime = DateTimeOffset.Now;
            var exists = _emailTemplateRepository.Query().Any(et => et.Name == emailTemplate.Name);
            if (exists)
            {
                rs.Result = Result.Failed;
                rs.Message = "Mẫu đã tồn tại";
                return rs;
            }
            await _emailTemplateRepository.InsertAsync(emailTemplate);
            try
            {
                _emailTemplateRepository.SaveChange();
            }
            catch (Exception ex)
            {
                rs.Result = Result.SystemError;
                rs.Message = ex.ToString();
            }
            return rs;
        }


        private async Task<BaseResult> Update(EmailTemplate emailTemplateItem)
        {

            var rs = new BaseResult() { Result = Result.Success };
            var emailTemplate = _emailTemplateRepository.Query().Where(et => !et.IsDeleted).FirstOrDefault(et => et.Id == emailTemplateItem.Id);
            if (emailTemplate != null)
            {
                emailTemplate.Name = emailTemplateItem.Name.Trim();
                emailTemplate.FilePath = emailTemplateItem.FilePath;
                emailTemplate.Drive = emailTemplateItem.Drive;
                emailTemplate.UpdatedTime = DateTimeOffset.Now;

                await _emailTemplateRepository.UpdateAsync(emailTemplate);
                try
                {
                    _emailTemplateRepository.SaveChange();
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

        public async Task<BaseResult> CreateOrUpdate(EmailTemplateItem emailTemplateItem)
        {
            var emailTemplate = emailTemplateItem.ToEmailTemplate();
            var rs = new BaseResult() { Result = Result.Success };
            return emailTemplate.Id <= 0 ? await Create(emailTemplate) : await Update(emailTemplate);

        }

        public async Task<BaseResult> Delete(long emailTemplateId)
        {
            var rs = new BaseResult() { Result = Result.Success };
            if (emailTemplateId > 0)
            {
                var emailTemplate = _emailTemplateRepository.Query().FirstOrDefault(et => et.Id == emailTemplateId);
                if (emailTemplate != null)
                {
                    var emails = _emailRepository.Query().Where(a => a.TemplateId == emailTemplateId).ToList();
                    foreach (var email in emails)
                    {
                        await _emailService.Delete(email.Id);
                    }
                    await _emailTemplateRepository.DeleteAsync(emailTemplate);
                    try
                    {
                        _emailTemplateRepository.SaveChange();
                    }
                    catch (Exception ex)
                    {
                        rs.Result = Result.SystemError;
                        rs.Message = ex.ToString();
                    }
                }
                else
                {
                    rs.Message = "Không tìm thấy mẫu yêu cầu!";
                    rs.Result = Result.Failed;
                }

            }
            else
            {
                rs.Message = "Mã mẫu không hợp lệ!";
                rs.Result = Result.Failed;
            }
            return rs;
        }
        #endregion
    }
}
