using System.Linq;
using System.Threading.Tasks;
using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Domain.Attributes;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Items.Orders;
using Kimerce.Backend.Dto.Items.Attributes;
using Kimerce.Backend.Dto.Models.Attributes;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.EntityFrameworkCore;
using System;

namespace Kimerce.Backend.Infrastructure.Services.Attributes
{
    public interface IAttributeService
    {
        Task<SmartTableResult<AttributeItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(AttributeModel model, int updateBy = 0, string updateByUserName = "");
        AttributeModel Get(int id);
        Task<BaseResult> Delete(int id);
    }

    public class AttributeService : IAttributeService
    {
        private readonly IRepository<Domain.Attributes.Attribute> _attributeRepository;

        public AttributeService(IRepository<Domain.Attributes.Attribute> attributeRepository)
        {
            _attributeRepository = attributeRepository;
        }

        public async Task<SmartTableResult<AttributeItem>> Search(SmartTableParam param)
        {
            var query = _attributeRepository.Query();
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

        public AttributeModel Get(int id)
        {
            return id > 0 ? _attributeRepository.GetById(id).ToModel() : new AttributeModel();

        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var AttributeForDelete = _attributeRepository.GetById(id);
            if (AttributeForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có thì xóa làm sao (o_ _)ﾉ彡☆";
                return result;
            }
            try
            {
                await _attributeRepository.DeleteAsync(AttributeForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }



        public async Task<BaseResult> CreateOrUpdate(AttributeModel model, int updateBy = 0, string updateByUserName = "")
        {
            var Attribute = model.ToAttribute();

            //Cập nhật thông tin chung của thực thể
            Attribute = Attribute.UpdateCommonInt(updateBy, updateByUserName);

            if (Attribute.Id > 0)
            {
                //Cập nhật
                return await Update(Attribute);
            }
            else
            {
                //Thêm mới
                return await Create(Attribute);
            }
        }

        private async Task<BaseResult> Update(Domain.Attributes.Attribute Attribute, int updateBy = 0, string updateByUserName = "")
        {
            var result = new CreateOrUpdateResultInt() { Result = Result.Success };
            var AttributeForUpdate = _attributeRepository.Query().FirstOrDefault(p => p.Id == Attribute.Id);
            if (AttributeForUpdate == null || Attribute.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy sản phẩm yêu cầu!";
                return result;
            }
            try
            {
                AttributeForUpdate = Attribute.ToAttribute(AttributeForUpdate);

                //Cập nhật thông tin chung của thực thể
                AttributeForUpdate = AttributeForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await _attributeRepository.UpdateAsync(AttributeForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = Attribute.Id;
            return result;
        }

        private async Task<BaseResult> Create(Domain.Attributes.Attribute Attribute)
        {
            var result = new CreateOrUpdateResultInt();
            Attribute.Name = Attribute.Name.Trim();
            try
            {
                await _attributeRepository.InsertAsync(Attribute);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = Attribute.Id;
            return result;
        }


        #endregion
    }
}