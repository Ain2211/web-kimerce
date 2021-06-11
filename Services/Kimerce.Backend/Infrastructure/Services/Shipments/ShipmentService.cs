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

namespace Kimerce.Backend.Infrastructure.Services.Shipments
{
    public interface IShipmentService
    {
        Task<Shipment_Item> GetbyId(int Id);
        Task<SmartTableResult<Shipment_Item>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(ShipmentModel model);
        BaseResult Delete(int id);
    }
    public class ShipmentService : IShipmentService
    {
        private readonly Repository<Shipment> _shipmentRepository;
        public ShipmentService(Repository<Shipment> repository)
        {
            _shipmentRepository = repository;
        }

        public async Task<Shipment_Item> GetbyId(int Id)
        {
            Shipment result = await _shipmentRepository.GetByIdAsync(Id);
            return result.ToItem();
        }

        public async Task<SmartTableResult<Shipment_Item>> Search(SmartTableParam param)
        {
            var query = _shipmentRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.OrderId.ToString().Contains(keyword));
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
        public async Task<BaseResult> CreateOrUpdate(ShipmentModel model)
        {
            Shipment shipment = model.ToShipment();
            if (shipment.Id > 0)
            {

                return await Update(shipment);
            }
            else
            {
                return await Create(shipment);
            }
        }
        private async Task<BaseResult> Create(Shipment shipment)
        {
            var rs = new BaseResult() { Result = Result.Success };

            var exists = _shipmentRepository.Query().Any(c => c.Id == shipment.Id);
            if (exists)
            {
                rs.Result = Result.Failed;
                rs.Message = "Đơn vận đã tồn tại!";
                return rs;
            }
            shipment.CreatedTime = DateTime.Now.ToLocalTime();
            try
            {

                await _shipmentRepository.InsertAsync(shipment);

            }
            catch (Exception ex)
            {
                rs.Result = Result.SystemError;
                rs.Message = ex.ToString();
            }
            return rs;
        }

        private async Task<BaseResult> Update(Shipment shipment)
        {
            var result = new BaseResult() { Result = Result.Success };
            var shipmentForUpdate = _shipmentRepository.Query().FirstOrDefault(p => p.Id == shipment.Id);
            if (shipmentForUpdate == null || shipment.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy đơn vận yêu cầu!";
                return result;
            }
            try
            {

                shipmentForUpdate.OrderId = shipment.OrderId;
                shipmentForUpdate.DeliveryStatus = shipment.DeliveryStatus;
                shipmentForUpdate.TrackingNumber = shipment.TrackingNumber;
                shipmentForUpdate.TotalWeight = shipment.TotalWeight;
                shipmentForUpdate.CompleteDate = shipment.CompleteDate;
                shipmentForUpdate.HandoverDate = shipment.HandoverDate;
                await _shipmentRepository.UpdateAsync(shipmentForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;


        }


        public BaseResult Delete(int Id)
        {
            var rs = new BaseResult() { Result = Result.Success };
            if (Id > 0)
            {
                var shipment = _shipmentRepository.GetById(Id);
                if (shipment != null)
                {
                    shipment.IsDeleted = true;
                    _shipmentRepository.Update(shipment);
                    try
                    {
                        _shipmentRepository.SaveChange();
                    }
                    catch (Exception ex)
                    {
                        rs.Result = Result.SystemError;
                        rs.Message = ex.ToString();
                    }
                }
                else
                {
                    rs.Message = "Không tìm thấy đơn hàng yêu cầu!";
                    rs.Result = Result.Failed;
                }

            }
            else
            {
                rs.Message = "Mã đơn hàng không hợp lệ!";
                rs.Result = Result.Failed;
            }
            return rs;
        }
    }
}
