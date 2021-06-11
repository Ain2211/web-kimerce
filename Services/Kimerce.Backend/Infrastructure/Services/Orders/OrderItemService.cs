using Kimerce.Backend.Domain.Orders;
using Kimerce.Backend.Domain.Products;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Orders;
using Kimerce.Backend.Dto.Models.Orders;
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

namespace Kimerce.Backend.Infrastructure.Services.Orders
{
    public interface IOrderItemService
    {
         int GetValue(int id);
        Task<BaseResult> DeleteByOrder(int id);
        Domain.Orders.Order_Item Get(int id);
        Task<SmartTableResult<OrderItemItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(OrderItemModel order, int updateBy = 0, string updateByUserName = "");
        Task<BaseResult> Delete(int id);
        Task<ActionResult<IEnumerable<Domain.Orders.Order_Item>>> Index();
        IQueryable<Product> GetProductsByOrderId(int id);
        IQueryable<Dto.Items.Orders.OrderItem> GetOrdersByProductId(int id);
        Task<BaseResult> DeleteProductInOrder(int id1, int id2);
    }
    public class OrderItemService : IOrderItemService
    {

        private readonly IRepository<Domain.Orders.Order_Item> _Repository;
        public OrderItemService(IRepository<Domain.Orders.Order_Item> Repository)
        {
            this._Repository = Repository;
        }

        public Domain.Orders.Order_Item Get(int id)
        {
            return _Repository.GetById(id);
        }

        public async Task<ActionResult<IEnumerable<Domain.Orders.Order_Item>>> Index()
        {
            return await _Repository.Query().ToListAsync();

        }

        public async Task<SmartTableResult<Dto.Items.Orders.OrderItemItem>> Search(SmartTableParam param)
        {
            var query = _Repository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.IdOrder.ToString().Contains(keyword));
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
            //  param.Sort = new Sort() { Predicate = "DisplayOrder", Reverse = false };
            var gridData = query.ToSmartTableResult(param, x => x.ToItem());
            return gridData;
        }
        public async Task<BaseResult> Delete(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var Delete = _Repository.GetById(id);
            if (Delete == null)
            {
                result.Result = Result.Failed;
                result.Message = "Không có ";
                return result;
            }
            try
            {
                await _Repository.DeleteAsync(Delete);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        public async Task<BaseResult> Update(Domain.Orders.Order_Item orderitem, int updateBy = 0, string updateByUserName = "")
        {

            var rs = new BaseResult() { Result = Result.Success };
            var orderForUpdate = _Repository.Query().FirstOrDefault(p => p.Id == orderitem.Id);
            if (orderForUpdate != null)
            {

                try
                {
                    orderForUpdate = orderitem.ToOrderItem(orderForUpdate);

                    orderForUpdate = orderForUpdate.UpdateCommonInt(updateBy, updateByUserName);
                    await _Repository.UpdateAsync(orderForUpdate);
                    _Repository.SaveChange();
                }
                catch (Exception ex)
                {
                    rs.Result = Result.SystemError;
                    rs.Message = ex.ToString();
                }
            }
            else
            {
                rs.Message = "Không tìm thấy don hang cần sửa";
                rs.Result = Result.Failed;
            }
            return rs;
        }

        public async Task<BaseResult> Create(Domain.Orders.Order_Item orderitem)
        {
            var rs = new BaseResult() { Result = Result.Success };



            try
            {
                await _Repository.InsertAsync(orderitem);
            }
            catch (Exception ex)
            {
                rs.Result = Result.SystemError;
                rs.Message = ex.ToString();
            }
            return rs;
        }

        public async Task<BaseResult> CreateOrUpdate(OrderItemModel model, int updateBy = 0, string updateByUserName = "")
        {
            var order = model.ToOrderItem();
            order.CreatedTime = DateTime.Now;
            if (order.Id > 0)
            {
                //Cập nhật
                return await Update(order);
            }
            else
            {
                //Thêm mới
                return await Create(order);
            }

        }

        public int GetValue (int id)
        {
            int val = 0;

            var query = _Repository.Query().Where( c => c.Id == id).ToList();
            foreach (Domain.Orders.Order_Item d in query)
            {
                val += (int)d.Price*d.NumberOfProduct;
            }

            return val;
        }

        public async Task<BaseResult> DeleteByOrder(int id)
        {
            var result = new BaseResult() { Result = Result.Success };
            var query = _Repository.Query().Where(c => c.IdOrder == id).ToList();
            foreach (Domain.Orders.Order_Item d in query)
            {
                var resulttmp = new BaseResult() { Result = Result.Success };
                resulttmp = await Delete(d.Id);
                if (resulttmp.Result == Result.Failed)
                {
                    result = await Delete(d.Id);
                }
                    
            }
            return result;
        }

        public IQueryable<Product> GetProductsByOrderId(int id)
        {
            var result = _Repository.Query().Where(x => x.IdOrder == id).Select(x => x.Product);
            return result;

        }

        public IQueryable<Dto.Items.Orders.OrderItem> GetOrdersByProductId(int id)
        {
            var result = _Repository.Query().Where(x => x.IdProduct == id).Select(x => x.Order);

            return result.Select(x => x.ToItem());

        }
        public async Task <BaseResult> DeleteProductInOrder(int id1, int id2)
        {
            var rs = new BaseResult() { Result = Result.Success };
            var orderitemForUpdate = _Repository.Query().FirstOrDefault(p => p.IdOrder == id1 & p.IdProduct == id2);

            return await this.Delete(orderitemForUpdate.Id);
        }
    }
}
