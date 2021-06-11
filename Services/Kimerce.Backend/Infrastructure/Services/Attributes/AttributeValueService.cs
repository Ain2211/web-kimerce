using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Domain.Attributes;
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

namespace Kimerce.Backend.Infrastructure.Services.AttributeValues
{
    public interface IAttributeValueService
    {
        Task<SmartTableResult<AttributeValueItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(AttributeValueModel model, int updateBy = 0, string updateByUserName = "");
        AttributeValueModel Get(int id);
        Task<BaseResult> Delete(int id);
    }

    public class AttributeValueService : IAttributeValueService
    {
        private readonly IRepository<AttributeValue> _attributeValueRepository;

        public AttributeValueService(IRepository<AttributeValue> attributeValueRepository)
        {
            _attributeValueRepository = attributeValueRepository;
        }

        public async Task<SmartTableResult<AttributeValueItem>> Search(SmartTableParam param)
        {
            var query = _attributeValueRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.Value.Contains(keyword));
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

        public AttributeValueModel Get(int id)
        {
            return id > 0 ? _attributeValueRepository.GetById(id).ToModel() : new AttributeValueModel();

        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var AttributeValueForDelete = _attributeValueRepository.GetById(id);
            if (AttributeValueForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có thì xóa làm sao (o_ _)ﾉ彡☆";
                return result;
            }
            try
            {
                await _attributeValueRepository.DeleteAsync(AttributeValueForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }



        public async Task<BaseResult> CreateOrUpdate(AttributeValueModel model, int updateBy = 0, string updateByUserName = "")
        {
            var AttributeValue = model.ToAttributeValue();

            //Cập nhật thông tin chung của thực thể
            AttributeValue = AttributeValue.UpdateCommonInt(updateBy, updateByUserName);

            if (AttributeValue.Id > 0)
            {
                //Cập nhật
                return await Update(AttributeValue);
            }
            else
            {
                //Thêm mới
                return await Create(AttributeValue);
            }
        }

        private async Task<BaseResult> Update(AttributeValue AttributeValue, int updateBy = 0, string updateByUserName = "")
        {
            var result = new CreateOrUpdateResultInt() { Result = Result.Success };
            var AttributeValueForUpdate = _attributeValueRepository.Query().FirstOrDefault(p => p.Id == AttributeValue.Id);
            if (AttributeValueForUpdate == null || AttributeValue.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy sản phẩm yêu cầu!";
                return result;
            }
            try
            {
                AttributeValueForUpdate = AttributeValue.ToAttributeValue(AttributeValueForUpdate);

                //Cập nhật thông tin chung của thực thể
                AttributeValueForUpdate = AttributeValueForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await _attributeValueRepository.UpdateAsync(AttributeValueForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = AttributeValue.Id;
            return result;
        }

        private async Task<BaseResult> Create(AttributeValue AttributeValue)
        {
            var result = new CreateOrUpdateResultInt();
            try
            {
                await _attributeValueRepository.InsertAsync(AttributeValue);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = AttributeValue.Id;
            return result;
        }


        #endregion
    }
}