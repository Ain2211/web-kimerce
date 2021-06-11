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

namespace Kimerce.Backend.Infrastructure.Services.InventoryAttributes
{
    public interface IInventoryAttributeService
    {
        Task<SmartTableResult<InventoryAttributeItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(InventoryAttributeModel model, int updateBy = 0, string updateByUserName = "");
        InventoryAttributeModel Get(int id);
        Task<BaseResult> Delete(int id);
    }

    public class InventoryAttributeService : IInventoryAttributeService
    {
        private readonly IRepository<InventoryAttribute> _inventoryAttributeRepository;

        public InventoryAttributeService(IRepository<InventoryAttribute> inventoryAttributeRepository)
        {
            _inventoryAttributeRepository = inventoryAttributeRepository;
        }

        public async Task<SmartTableResult<InventoryAttributeItem>> Search(SmartTableParam param)
        {
            var query = _inventoryAttributeRepository.Query();
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

        public InventoryAttributeModel Get(int id)
        {
            return id > 0 ? _inventoryAttributeRepository.GetById(id).ToModel() : new InventoryAttributeModel();

        }

        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var InventoryAttributeForDelete = _inventoryAttributeRepository.GetById(id);
            if (InventoryAttributeForDelete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có thì xóa làm sao (o_ _)ﾉ彡☆";
                return result;
            }
            try
            {
                await _inventoryAttributeRepository.DeleteAsync(InventoryAttributeForDelete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }



        public async Task<BaseResult> CreateOrUpdate(InventoryAttributeModel model, int updateBy = 0, string updateByUserName = "")
        {
            var InventoryAttribute = model.ToInventoryAttribute();

            //Cập nhật thông tin chung của thực thể
            InventoryAttribute = InventoryAttribute.UpdateCommonInt(updateBy, updateByUserName);

            if (InventoryAttribute.Id > 0)
            {
                //Cập nhật
                return await Update(InventoryAttribute);
            }
            else
            {
                //Thêm mới
                return await Create(InventoryAttribute);
            }
        }

        private async Task<BaseResult> Update(InventoryAttribute InventoryAttribute, int updateBy = 0, string updateByUserName = "")
        {
            var result = new CreateOrUpdateResultInt() { Result = Result.Success };
            var InventoryAttributeForUpdate = _inventoryAttributeRepository.Query().FirstOrDefault(p => p.Id == InventoryAttribute.Id);
            if (InventoryAttributeForUpdate == null || InventoryAttribute.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy sản phẩm yêu cầu!";
                return result;
            }
            try
            {
                InventoryAttributeForUpdate = InventoryAttribute.ToInventoryAttribute(InventoryAttributeForUpdate);

                //Cập nhật thông tin chung của thực thể
                InventoryAttributeForUpdate = InventoryAttributeForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await _inventoryAttributeRepository.UpdateAsync(InventoryAttributeForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = InventoryAttribute.Id;
            return result;
        }

        private async Task<BaseResult> Create(InventoryAttribute InventoryAttribute)
        {
            var result = new CreateOrUpdateResultInt();
            InventoryAttribute.Name = InventoryAttribute.Name.Trim();
            try
            {
                await _inventoryAttributeRepository.InsertAsync(InventoryAttribute);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            result.Id = InventoryAttribute.Id;
            return result;
        }


        #endregion
    }
}