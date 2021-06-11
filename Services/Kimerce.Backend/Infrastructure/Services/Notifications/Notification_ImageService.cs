using Kimerce.Backend.Domain.Images;
using Kimerce.Backend.Domain.Notification;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Images;
using Kimerce.Backend.Dto.Models.Notification;
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


namespace Kimerce.Backend.Infrastructure.Services.Notifications
{
    
        public interface INotification_ImageService
        {

           
            Notification_Image Get(int id);
            Task<SmartTableResult<Notification_Image>> Search(SmartTableParam param);
            Task<BaseResult> CreateOrUpdate(Notification_ImageModel image, int updateBy = 0, string updateByUserName = "");
            Task<BaseResult> Delete(int id);
            Task<ActionResult<IEnumerable<Notification_Image>>> Index();
            IQueryable<Notification> GgetNotificationByImageId(int id);
            IQueryable<ImageItem> GetImageByNotificationId(int id);

        }


        public class Notification_ImageService : INotification_ImageService
    {

            private readonly IRepository<Notification_Image> _Repository;
            public Notification_ImageService(IRepository<Notification_Image> Repository)
            {
                this._Repository = Repository;
            }

            public Notification_Image Get(int id)
            {
                return _Repository.GetById(id);
            }

            public async Task<ActionResult<IEnumerable<Notification_Image>>> Index()
            {
                return await _Repository.Query().ToListAsync();

            }

            public async Task<SmartTableResult<Notification_Image>> Search(SmartTableParam param)
            {
                var query = _Repository.Query();
                if (param.Search.PredicateObject != null)
                {
                    dynamic search = param.Search.PredicateObject;
                    if (search.Keyword != null)
                    {
                        string keyword = search.Keyword;
                        keyword = keyword.Trim().ToLower();
                        query = query.Where(x => x.ImageId.ToString().Contains(keyword));
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
                var gridData = query.ToSmartTableResult(param, x => x);
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

            public async Task<BaseResult> Update(Notification_Image notification_Image, int updateBy = 0, string updateByUserName = "")
            {

                var rs = new BaseResult() { Result = Result.Success };
                var notificationforUpdate = _Repository.Query().FirstOrDefault(p => p.Id == notification_Image.Id);
                if (notificationforUpdate != null)
                {

                    try
                    {
                        notificationforUpdate = notification_Image.ToNotification_Image(notificationforUpdate);
                        notificationforUpdate.NotiId = notification_Image.NotiId;
                        notificationforUpdate.ImageId = notification_Image.ImageId;
                        notificationforUpdate.notification = notification_Image.notification;
                        notificationforUpdate.image = notification_Image.image;
                        notificationforUpdate = notificationforUpdate.UpdateCommonInt(updateBy, updateByUserName);
                        await _Repository.UpdateAsync(notificationforUpdate);
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

            public async Task<BaseResult> Create(Notification_Image orderitem)
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

            public async Task<BaseResult> CreateOrUpdate(Notification_ImageModel model, int updateBy = 0, string updateByUserName = "")
            {
                var order = model.ToNotification_Image();
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



            public async Task<BaseResult> DeleteByImageId(int id)
            {
                var result = new BaseResult() { Result = Result.Success };
                var query = _Repository.Query().Where(c => c.ImageId == id).ToList();
                foreach (Notification_Image d in query)
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

            public IQueryable<Notification> GgetNotificationByImageId(int id)
            {
                var result = _Repository.Query().Where(x => x.ImageId == id).Select(x => x.notification);
                return result;

            }

            public IQueryable<ImageItem> GetImageByNotificationId(int id)
            {
                var result = _Repository.Query().Where(x => x.NotiId == id).Select(x => x.image);

                return result.Select(x => x.ToItem());

            }

        }
    }

