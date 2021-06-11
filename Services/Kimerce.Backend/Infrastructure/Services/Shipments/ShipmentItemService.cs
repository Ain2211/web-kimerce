using Kimerce.Backend.Domain.Shipments;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Shipments;
using Kimerce.Backend.Dto.Models.Shipments;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Services.ShipmentItems
{
    public interface IShipmentItemService
    {
        

    }
    public class ShipmentItemService : IShipmentItemService
    {
        private readonly Repository<ShipmentItem> _shipmentItemRepository;
        public ShipmentItemService(Repository<ShipmentItem> repository)
        {
            _shipmentItemRepository = repository;
        }
        public async Task<SmartTableResult<ShipmentItem_Item>> Search(SmartTableParam param)
        {
            var query = _shipmentItemRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.ShipmentId.ToString().Contains(keyword));
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

            var gridData = query.ToSmartTableResult(param, x => x.ToItem());
            return gridData;
        }
        public async Task<ShipmentItem> GetbyId(int Id)
        {
            return await _shipmentItemRepository.GetByIdAsync(Id);
        }
        public async Task<BaseResult> CreateOrUpdate(ShipmentItemModel model)
        {
            ShipmentItem shipmentItem = model.ToShipmentItem();
            if (shipmentItem.Id > 0)
            {
                return await Update(shipmentItem.Id, shipmentItem);
            }
            else
            {
                return await Create(shipmentItem);
            }
        }
      
        public async Task<BaseResult> Create(ShipmentItem shipmentItem)
        {
            var rs = new BaseResult() { Result = Result.Success };

            var exists = _shipmentItemRepository.Query().Any(c => c.Id == shipmentItem.Id);
            if (exists)
            {
                rs.Result = Result.Failed;
                rs.Message = "Tỉnh / thành phố đã tồn tại!";
                return rs;
            }
            shipmentItem.CreatedTime = DateTime.Now.ToLocalTime();
            try
            {
                await _shipmentItemRepository.InsertAsync(shipmentItem);

            }
            catch (Exception ex)
            {
                rs.Result = Result.SystemError;
                rs.Message = ex.ToString();
            }
            return rs;
        }

        public async Task<BaseResult> Update(int Id, ShipmentItem shipmentItem)
        {

            var rs = new BaseResult() { Result = Result.Success };
            var shipmentItemForUpdate = _shipmentItemRepository.Query().FirstOrDefault(p => p.Id == shipmentItem.Id);

            if (shipmentItemForUpdate == null || shipmentItem.Id <= 0)
            {
                rs.Result = Result.Failed;
                rs.Message = "Không tìm thấy đơn vận yêu cầu!";
                return rs;
            }
            try
            {
                        //shipmentItem = shipmentItem.ToShipmentItem(shipmentItemItem);
                        shipmentItemForUpdate.ShipmentId = shipmentItem.ShipmentId;
                        shipmentItemForUpdate.Quantity = shipmentItem.Quantity;
                        shipmentItemForUpdate.OrderItemId = shipmentItem.OrderItemId;
                        shipmentItemForUpdate.WareHouseId = shipmentItem.WareHouseId;
                        await _shipmentItemRepository.UpdateAsync(shipmentItemForUpdate);
            }
                   
            catch (Exception e)
            {
                rs.Result = Result.SystemError;
                rs.Message = e.ToString();
            }
            
            return rs;
        }


        public BaseResult Delete(int Id)
        {
            var rs = new BaseResult() { Result = Result.Success };
            if (Id > 0)
            {
                var shipmentItem = _shipmentItemRepository.GetById(Id);
                if (shipmentItem != null)
                {
                    shipmentItem.IsDeleted = true;
                    _shipmentItemRepository.Update(shipmentItem);
                    try
                    {
                        _shipmentItemRepository.SaveChange();
                    }
                    catch (Exception ex)
                    {
                        rs.Result = Result.SystemError;
                        rs.Message = ex.ToString();
                    }
                }
                else
                {
                    rs.Message = "Không tìm thấy tỉnh / thành phố yêu cầu!";
                    rs.Result = Result.Failed;
                }

            }
            else
            {
                rs.Message = "Mã tỉnh / thành phố không hợp lệ!";
                rs.Result = Result.Failed;
            }
            return rs;
        }
    }
}
