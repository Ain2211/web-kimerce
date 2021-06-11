using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.Orders;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.EntityFrameworkCore;
using System;
using Kimerce.Backend.Dto.Items.Attributes;
using Kimerce.Backend.Dto.Models.Attributes;
using Kimerce.Backend.Domain.Attributes;

namespace Kimerce.Backend.Infrastructure.Services.SpecAttributes
{
    public interface ISpecAttributeService
    {
        Task<SmartTableResult<SpecAttributeItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(SpecAttributeModel model, int updateBy = 0, string updateByUserName = "");
        SpecAttributeModel Get(int id);
        Task<BaseResult> Delete(int id);
    }

    public class SpecAttributeService : ISpecAttributeService
    {
        private readonly IRepository<SpecAttribute> _specAttributeRepository;
        //private readonly IRepository<OrderItem> _orderItemRepository;

        public SpecAttributeService(IRepository<SpecAttribute> specAttributeRepository)
        {
            _specAttributeRepository = specAttributeRepository;
        }

        public async Task<SmartTableResult<SpecAttributeItem>> Search(SmartTableParam param)
        {
            var query = _specAttributeRepository.Query();
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

        public SpecAttributeModel Get(int id)
        {
            return id > 0 ? _specAttributeRepository.GetById(id).ToModel() : new SpecAttributeModel();

        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var SpecAttributeForDelete = _specAttributeRepository.GetById(id);
            if (SpecAttributeForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có thì xóa làm sao (o_ _)ﾉ彡☆";
                return result;
            }
            try
            {
                await _specAttributeRepository.DeleteAsync(SpecAttributeForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }



        public async Task<BaseResult> CreateOrUpdate(SpecAttributeModel model, int updateBy = 0, string updateByUserName = "")
        {
            var SpecAttribute = model.ToSpecAttribute();

            //Cập nhật thông tin chung của thực thể
            SpecAttribute = SpecAttribute.UpdateCommonInt(updateBy, updateByUserName);

            if (SpecAttribute.Id > 0)
            {
                //Cập nhật
                return await Update(SpecAttribute);
            }
            else
            {
                //Thêm mới
                return await Create(SpecAttribute);
            }
        }

        private async Task<BaseResult> Update(SpecAttribute SpecAttribute, int updateBy = 0, string updateByUserName = "")
        {
            var result = new CreateOrUpdateResultInt() { Result = Result.Success };
            var SpecAttributeForUpdate = _specAttributeRepository.Query().FirstOrDefault(p => p.Id == SpecAttribute.Id);
            if (SpecAttributeForUpdate == null || SpecAttribute.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy sản phẩm yêu cầu!";
                return result;
            }
            try
            {
                SpecAttributeForUpdate = SpecAttribute.ToSpecAttribute(SpecAttributeForUpdate);

                //Cập nhật thông tin chung của thực thể
                SpecAttributeForUpdate = SpecAttributeForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await _specAttributeRepository.UpdateAsync(SpecAttributeForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = SpecAttribute.Id;
            return result;
        }

        private async Task<BaseResult> Create(SpecAttribute SpecAttribute)
        {
            var result = new CreateOrUpdateResultInt();
            SpecAttribute.Name = SpecAttribute.Name.Trim();
            try
            {
                await _specAttributeRepository.InsertAsync(SpecAttribute);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = SpecAttribute.Id;
            return result;
        }


        #endregion
    }
}