using GemBox.Spreadsheet;
using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Domain.Locations;
using Kimerce.Backend.Domain.WareHouse;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Categories;
using Kimerce.Backend.Dto.Items.Categories.WareHouse;
using Kimerce.Backend.Dto.Models.Categories;
using Kimerce.Backend.Dto.Results;
using Kimerce.Backend.Infrastructure.Helpers;
using Kimerce.Backend.Infrastructure.Repositories;
using Kimerce.Backend.Infrastructure.SmartTable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kimerce.Backend.Infrastructure.Services.Categories
{
    public interface IWareHouseService
    {
        Task<SmartTableResult<WareHouseItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(WareHouseModel model, int updateBy = 0, string updateByUserName = "");
        WareHouseModel Get(int id);
        Task<BaseResult> Delete(int id);
        Task<BaseResult> ExportToXlsx(SmartTableParam param);

    }

    public class WareHouseService : IWareHouseService
    {
        private readonly IRepository<WareHouse> _wareHouseRepository;
        private readonly IRepository<Ward> _wardRepository;
        public WareHouseService(IRepository<WareHouse> wareHouseRepository, IRepository<Ward> wardRepository)
        {
            this._wareHouseRepository = wareHouseRepository;
            this._wardRepository = wardRepository;
        }

        /// <summary>
        /// Tìm kiếm danh sách DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<SmartTableResult<WareHouseItem>> Search(SmartTableParam param)
        {
            var query = _wareHouseRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(keyword) 
                                            || x.Address.ToLower().Contains(keyword)
                                            || x.Latitude.ToString().Contains(keyword)
                                            || x.Longtitude.ToString().Contains(keyword)
                                            || x.PhoneNumber.Contains(keyword)
                                            || x.ContactName.ToLower().Contains(keyword));
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

                if (search.Realm != 0 && search.Realm != null)
                {
                    CityRealm realm = search.Realm;
                    query = query.Where(x => x.Ward.CityRealm == realm);
                }
            }
            param.Sort = new Sort() { Predicate = "DisplayOrder", Reverse = false };
            var gridData = query.ToSmartTableResult(param, x => x.ToItem());
            return gridData;
        }

        #region CRUD
        public WareHouseModel Get(int id)
        {
            return id > 0 ? _wareHouseRepository.Query()
                                                .Include(w =>w.Ward)
                                                .ThenInclude(w => w.District)
                                                .ThenInclude(d => d.City)
                                                .FirstOrDefault(w => w.Id == id).ToModel() : new WareHouseModel();
        }

        public async Task<BaseResult> CreateOrUpdate(WareHouseModel model, int updateBy = 0, string updateByUserName = "")
        {
            var wareHouse = model.ToWareHouse();

            //Cập nhật thông tin chung của thực thể
            wareHouse = wareHouse.UpdateCommonInt(updateBy, updateByUserName);

            if (wareHouse.Id > 0)
            {
                //Cập nhật
                return await Update(wareHouse);
            }
            else
            {
                //Thêm mới
                return await Create(wareHouse);
            }
        }

        private async Task<BaseResult> Update(WareHouse wareHouse, int updateBy = 0, string updateByUserName = "")
        {
            var result = new BaseResult() { Result = Result.Success };
            var wareHouseForUpdate = _wareHouseRepository.Query()
                .FirstOrDefault(c => c.Id == wareHouse.Id);
            if (wareHouseForUpdate == null || wareHouse.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy nhà kho yêu cầu!";
                return result;
            }
            else
            {
                Ward ward = _wardRepository.Query()
                            .Include(w => w.District)
                            .ThenInclude(d => d.City)
                            .AsNoTracking()
                            .FirstOrDefault(c => c.Id == wareHouse.ParentId);

                var exists = _wareHouseRepository.Query().Any(c => !c.IsDeleted && c.Name == wareHouse.Name && c.Id != wareHouseForUpdate.Id);
                if (exists)
                {
                    result.Result = Result.Failed;
                    result.Message = "Nhà kho đã tồn tại!";
                    return result;
                }
                wareHouse.Name = wareHouse.Name.Trim();
                wareHouse.PhoneNumber = wareHouse.PhoneNumber.Trim();
                wareHouse.ContactName = wareHouse.ContactName.Trim();
                wareHouse.Address = ward.Name + ", " + ward.District.Name + ", " + ward.District.City.Name;
            }
            try
            {
                wareHouseForUpdate = wareHouse.ToWareHouse(wareHouseForUpdate);

                //Cập nhật thông tin chung của thực thể
                wareHouseForUpdate = wareHouseForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await _wareHouseRepository.UpdateAsync(wareHouseForUpdate);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        private async Task<BaseResult> Create(WareHouse wareHouse)
        {
            var result = new BaseResult();
            var ward = _wardRepository.Query()
                            .Include(w => w.District)
                            .ThenInclude(d => d.City)
                            .FirstOrDefault(c => c.Id == wareHouse.ParentId);
            wareHouse.Name = wareHouse.Name.Trim();
            wareHouse.PhoneNumber = wareHouse.PhoneNumber.Trim();
            wareHouse.ContactName = wareHouse.ContactName.Trim();
            wareHouse.Address = ward.Name + ", " + ward.District.Name + ", " + ward.District.City.Name;

            var exists = _wareHouseRepository.Query().Any(c => c.Name == wareHouse.Name);
            if (exists)
            {
                result.Result = Result.Failed;
                result.Message = "Nhà kho đã tồn tại!";
                return result;
            }
            try
            {
                await _wareHouseRepository.InsertAsync(wareHouse);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        public async Task<BaseResult> Delete(int wareHouseId)
        {
            var rs = new BaseResult() { Result = Result.Success };
            if (wareHouseId > 0)
            {
                var wareHouse = _wareHouseRepository.Query().FirstOrDefault(c => c.Id == wareHouseId);
                if (wareHouse != null)
                {
                    wareHouse.IsDeleted = true;
                    _wareHouseRepository.Update(wareHouse);
                    try
                    {
                        _wareHouseRepository.SaveChange();
                    }
                    catch (Exception ex)
                    {
                        rs.Result = Result.SystemError;
                        rs.Message = ex.ToString();
                    }
                }
                else
                {
                    rs.Message = "Không tìm thấy nhà kho yêu cầu!";
                    rs.Result = Result.Failed;
                }
            }
            else
            {
                rs.Message = "Mã nhà kho không hợp lệ!";
                rs.Result = Result.Failed;
            }
            return rs;
        }

        public async Task<BaseResult> ExportToXlsx(SmartTableParam param)
        {
            var rs = new BaseResult() { Result = Result.Success };

            SmartTableResult<WareHouseItem> warehouses = Search(param).Result;
            List<WareHouseItem> list = warehouses.Items.ToList();
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Quản lí nhà kho");


            var style = worksheet.Rows[0].Style;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            worksheet.Columns[0].SetWidth(50, LengthUnit.Pixel);
            worksheet.Columns[1].SetWidth(200, LengthUnit.Pixel);
            worksheet.Columns[2].SetWidth(200, LengthUnit.Pixel);
            worksheet.Columns[5].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[6].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[7].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[8].SetWidth(150, LengthUnit.Pixel);

            worksheet.Cells["A1"].Value = "ID";
            worksheet.Cells["B1"].Value = "Tên Nhà Kho";
            worksheet.Cells["C1"].Value = "Địa Chỉ";
            worksheet.Cells["D1"].Value = "Vĩ Độ";
            worksheet.Cells["E1"].Value = "Kinh Độ";
            worksheet.Cells["F1"].Value = "SĐT Người Liên Hệ";
            worksheet.Cells["G1"].Value = "Tên Người Liên Hệ";
            worksheet.Cells["H1"].Value = "Ngày Tạo";
            worksheet.Cells["I1"].Value = "Ngày Cập Nhật";

            worksheet.Tables.Add("Table1", "A1:I" + (list.Count + 1).ToString(), true);


            for (int r = 1; r <= list.Count; r++)
            {
                var item = list[r - 1];
                worksheet.Cells[r, 0].Value = item.Id;
                worksheet.Cells[r, 1].Value = item.Name;
                worksheet.Cells[r, 2].Value = item.Address;
                worksheet.Cells[r, 3].Value = item.Latitude;
                worksheet.Cells[r, 4].Value = item.Longtitude;
                worksheet.Cells[r, 5].Value = item.PhoneNumber;
                worksheet.Cells[r, 6].Value = item.ContactName;
                worksheet.Cells[r, 7].Value = item.CreatedTimeDisplay;
                worksheet.Cells[r, 8].Value = item.UpdatedTimeDisplay;
            }
            string fileName = "ExportFile.xlsx";
            workbook.Save(fileName);

            rs.Message = "Thành Công!";
            return rs;
        }

        #endregion
    }
}







