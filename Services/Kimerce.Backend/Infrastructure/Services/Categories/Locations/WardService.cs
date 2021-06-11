using GemBox.Spreadsheet;
using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Domain.Locations;
using Kimerce.Backend.Domain.WareHouse;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Items.Categories.WareHouse;
using Kimerce.Backend.Dto.Items.Locations;
using Kimerce.Backend.Dto.Models.Categories;
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

namespace Kimerce.Backend.Infrastructure.Services.Categories
{
    public interface IWardService
    {
        Task<SmartTableResult<WardItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(WardModel model, int updateBy = 0, string updateByUserName = "");
        WardModel Get(int id);
        IQueryable<WareHouseItem> GetChildren(int id);
        Task<BaseResult> Delete(int id);
        Task<BaseResult> ExportToXlsx(SmartTableParam param);
    }

    public class WardService : IWardService
    {
        private readonly IRepository<Ward> _wardRepository;
        private readonly IRepository<District> _districtRepository;
        private readonly IRepository<WareHouse> _wareHouseRepository;
        private readonly IWareHouseService _wareHouseService;

        public WardService(IRepository<Ward> wardRepository, IRepository<District> districtRepository,
                                    IRepository<WareHouse> wareHouseRepository, IWareHouseService wareHouseService)
        {
            _wardRepository = wardRepository;
            _districtRepository = districtRepository;
            _wareHouseRepository = wareHouseRepository;
            _wareHouseService = wareHouseService;
        }

        /// <summary>
        /// Tìm kiếm danh sách DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<SmartTableResult<WardItem>> Search(SmartTableParam param)
        {
            var query = _wardRepository.Query()
                            .Include(w => w.District)
                            .ThenInclude(d => d.City)
                            .AsNoTracking();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(keyword)
                                          || x.UnsignName.Contains(keyword)
                                          || x.Code.Contains(keyword)
                                          || x.District.Name.ToLower().Contains(keyword)
                                          || x.District.UnsignName.Contains(keyword)
                                          || x.District.City.Name.ToLower().Contains(keyword)
                                          || x.District.City.UnsignName.Contains(keyword));
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
                    query = query.Where(x => x.CityRealm == realm);
                }
            }
            param.Sort = new Sort() { Predicate = "DisplayOrder", Reverse = false };
            var gridData = query.ToSmartTableResult(param, x => x.ToItem());
            return gridData;
        }

        #region CRUD
        public WardModel Get(int id)
        {
            return id > 0 ? _wardRepository.Query()
                                           .Include(w => w.District)
                                           .ThenInclude(d => d.City)
                                           .FirstOrDefault(w => w.Id == id).ToModel() : new WardModel();
        }

        public IQueryable<WareHouseItem> GetChildren(int id)
        {
            return _wareHouseRepository.Query().Where(x => x.ParentId == id).Select(x => x.ToItem());

        }

        public async Task<BaseResult> CreateOrUpdate(WardModel model, int updateBy = 0, string updateByUserName = "")
        {
            var ward = model.ToWard();

            //Cập nhật thông tin chung của thực thể
            ward = ward.UpdateCommonInt(updateBy, updateByUserName);

            if (ward.Id > 0)
            {
                //Cập nhật
                return await Update(ward);
            }
            else
            {
                //Thêm mới
                return await Create(ward);
            }
        }

        private async Task<BaseResult> Update(Ward ward, int updateBy = 0, string updateByUserName = "")
        {
            var result = new BaseResult() { Result = Result.Success };
            var wardForUpdate = _wardRepository.Query().FirstOrDefault(c => c.Id == ward.Id);
            if (wardForUpdate == null || ward.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy phường yêu cầu!";
                return result;
            }
            else
            {
                var district = _districtRepository.Query()
                            .Include(d => d.City)
                            .Include(d => d.Wards)
                            .AsNoTracking()
                            .FirstOrDefault(d => d.Id == ward.ParentId);
                var exists = _wardRepository.Query()
                                            .Include(w => w.District)
                                            .ThenInclude(d => d.City)
                                            .AsNoTracking()
                                            .Any(w => !w.IsDeleted && w.District.Name == district.Name && w.Name == ward.Name && w.Id != wardForUpdate.Id);
                if (exists)
                {
                    result.Result = Result.Failed;
                    result.Message = "Xã/Phường đã tồn tại!";
                    return result;
                }
                ward.CityRealm = district.CityRealm;
                ward.Name = ward.Name.Trim();
                ward.UnsignName = !string.IsNullOrEmpty(ward.Name) ? ward.Name.Unsigned() : "";
            }
            try
            {
                wardForUpdate = ward.ToWard(wardForUpdate);

                //Cập nhật thông tin chung của thực thể
                wardForUpdate = wardForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await _wardRepository.UpdateAsync(wardForUpdate);

                var warehouses = _wareHouseRepository.Query()
                                              .Where(w => w.ParentId == wardForUpdate.Id)
                                              .Include(w => w.Ward)
                                              .ThenInclude(w => w.District)
                                              .ThenInclude(d => d.City)
                                              .ToList();
                foreach (WareHouse w in warehouses)
                {
                    WareHouseModel wareHouse = w.ToModel();
                    await _wareHouseService.CreateOrUpdate(wareHouse);
                }
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        private async Task<BaseResult> Create(Ward ward)
        {
            var result = new BaseResult();
            var district = _districtRepository.Query()
                            .Include(d => d.City)
                            .Include(d => d.Wards)
                            .AsNoTracking()
                            .FirstOrDefault(d => d.Id == ward.ParentId);
            var exists = _wardRepository.Query()
                                        .Include(w => w.District)
                                        .AsNoTracking()
                                        .Any(w => w.District.Name == district.Name && w.Name == ward.Name);
            if (exists)
            {
                result.Result = Result.Failed;
                result.Message = "Xã/Phường đã tồn tại!";
                return result;
            }
            ward.CityRealm = district.CityRealm;
            ward.Name = ward.Name.Trim();
            ward.UnsignName = !string.IsNullOrEmpty(ward.Name) ? ward.Name.Unsigned() : "";
            try
            {
                await _wardRepository.InsertAsync(ward);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        public async Task<BaseResult> Delete(int wardId)
        {
            var rs = new BaseResult() { Result = Result.Success };
            if (wardId > 0)
            {
                var ward = _wardRepository.Query().FirstOrDefault(c => c.Id == wardId);
                if (ward != null)
                {
                    ward.IsDeleted = true;

                    var wareHouses = _wareHouseRepository.Query()
                                                  .Include(w => w.Ward)
                                                  .Where(w => w.ParentId == wardId)
                                                  .ToList();
                    foreach (WareHouse w in wareHouses)
                    {
                        await _wareHouseService.Delete(w.Id);
                    }

                    _wardRepository.Update(ward);
                    try
                    {
                        _wardRepository.SaveChange();
                    }
                    catch (Exception ex)
                    {
                        rs.Result = Result.SystemError;
                        rs.Message = ex.ToString();
                    }
                }
                else
                {
                    rs.Message = "Không tìm thấy xã / phường yêu cầu!";
                    rs.Result = Result.Failed;
                }

            }
            else
            {
                rs.Message = "Mã xã / phường không hợp lệ!";
                rs.Result = Result.Failed;
            }
            return rs;
        }

        public async Task<BaseResult> ExportToXlsx(SmartTableParam param)
        {
            var rs = new BaseResult() { Result = Result.Success };

            SmartTableResult<WardItem> wards = Search(param).Result;
            List<WardItem> list = wards.Items.ToList();
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Quản lí xã, phường");


            var style = worksheet.Rows[0].Style;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            worksheet.Columns[0].SetWidth(50, LengthUnit.Pixel);
            worksheet.Columns[1].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[2].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[3].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[5].SetWidth(100, LengthUnit.Pixel);
            worksheet.Columns[6].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[7].SetWidth(150, LengthUnit.Pixel);

            worksheet.Cells["A1"].Value = "ID";
            worksheet.Cells["B1"].Value = "Tên Xã/Phường";
            worksheet.Cells["C1"].Value = "Huyện/Quận";
            worksheet.Cells["D1"].Value = "Tỉnh/Thành Phố";
            worksheet.Cells["E1"].Value = "Mã";
            worksheet.Cells["F1"].Value = "Miền";
            worksheet.Cells["G1"].Value = "Ngày Tạo";
            worksheet.Cells["H1"].Value = "Ngày Cập Nhật";

            worksheet.Tables.Add("Table1", "A1:H" + (list.Count + 1).ToString(), true);


            for (int r = 1; r <= list.Count; r++)
            {
                var item = list[r - 1];
                worksheet.Cells[r, 0].Value = item.Id;
                worksheet.Cells[r, 1].Value = item.Name;
                worksheet.Cells[r, 2].Value = item.District.Name;
                worksheet.Cells[r, 3].Value = item.District.City.Name;
                worksheet.Cells[r, 4].Value = item.Code;
                worksheet.Cells[r, 5].Value = item.CityRealmStr;
                worksheet.Cells[r, 6].Value = item.CreatedTimeDisplay;
                worksheet.Cells[r, 7].Value = item.UpdatedTimeDisplay;
            }
            string fileName = "ExportFile.xlsx";
            workbook.Save(fileName);

            rs.Message = "Thành Công!";
            return rs;
        }
        #endregion


    }
}
