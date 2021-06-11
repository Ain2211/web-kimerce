using GemBox.Spreadsheet;
using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Domain.Locations;
using Kimerce.Backend.Dto;
using Kimerce.Backend.Dto.Categories;
using Kimerce.Backend.Dto.Items.Locations;
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
    public interface ICityService
    {
        Task<SmartTableResult<CityItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(CityModel model, int updateBy = 0, string updateByUserName = "");
        CityModel Get(int id);
        IQueryable<DistrictItem> GetChildren(int id);
        Task<BaseResult> Delete(int id);
        Task<BaseResult> ExportToXlsx(SmartTableParam param);
    }

    public class CityService : ICityService
    {
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<District> _districtRepository;
        private readonly IDistrictService _districtService;
        public CityService(IRepository<City> cityRepository, IRepository<District> districtRepository, IDistrictService districtService)
        {
            this._cityRepository = cityRepository;
            this._districtRepository = districtRepository;
            this._districtService = districtService;
        }

        /// <summary>
        /// Tìm kiếm danh sách DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<SmartTableResult<CityItem>> Search(SmartTableParam param)
        {
            var query = _cityRepository.Query();
            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;
                if (search.Keyword != null)
                {
                    string keyword = search.Keyword;
                    keyword = keyword.Trim().ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(keyword) 
                                          || x.UnsignName.Contains(keyword)
                                          || x.Code.Contains(keyword));
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
                if (search.Realm != 0 && search.Realm !=null)
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
        public CityModel Get(int id)
        {
            return id > 0 ? _cityRepository.GetById(id).ToModel() : new CityModel();
        }

        public IQueryable<DistrictItem> GetChildren(int id)
        {
            return _districtRepository.Query().Where(x => x.ParentId == id).Select(x => x.ToItem());

        }

        public async Task<BaseResult> CreateOrUpdate(CityModel model, int updateBy = 0, string updateByUserName = "")
        {
            var city = model.ToCity();

            //Cập nhật thông tin chung của thực thể
            city = city.UpdateCommonInt(updateBy, updateByUserName);

            if (city.Id > 0)
            {
                //Cập nhật
                return await Update(city);
            }
            else
            {
                //Thêm mới
                return await Create(city);
            }
        }

        private async Task<BaseResult> Update(City city, int updateBy = 0, string updateByUserName = "")
        {
            var result = new BaseResult() { Result = Result.Success };
            var cityForUpdate = _cityRepository.Query().FirstOrDefault(c => c.Id == city.Id);
            if (cityForUpdate == null || city.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy thành phố yêu cầu!";
                return result;
            }
            else
            {
                var exists = _cityRepository.Query().Any(c => !c.IsDeleted && c.Name == city.Name && c.Id != cityForUpdate.Id);
                if (exists)
                {
                    result.Result = Result.Failed;
                    result.Message = "Tỉnh / thành phố đã tồn tại!";
                    return result;
                }
                city.Name = city.Name.Trim();
                city.UnsignName = !string.IsNullOrEmpty(city.Name) ? city.Name.Unsigned() : "";
            }
            try
            {
                cityForUpdate = city.ToCity(cityForUpdate);

                //Cập nhật thông tin chung của thực thể
                cityForUpdate = cityForUpdate.UpdateCommonInt(updateBy, updateByUserName);
                await _cityRepository.UpdateAsync(cityForUpdate);

                var districts = _districtRepository.Query()
                                                  .Include(d => d.City)
                                                  .Where(d => d.ParentId == cityForUpdate.Id)
                                                  .ToList();
                foreach (District d in districts)
                {
                    DistrictModel district = d.ToModel();
                    district.CityRealm = cityForUpdate.CityRealm;
                    await _districtService.CreateOrUpdate(district);
                }
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }

            return result;
        }

        private async Task<BaseResult> Create(City city)
        {
            var result = new BaseResult();
            city.Name = city.Name.Trim();
            city.UnsignName = !string.IsNullOrEmpty(city.Name) ? city.Name.Unsigned() : "";
            var exists = _cityRepository.Query().Any(c => c.Name == city.Name);
            if (exists)
            {
                result.Result = Result.Failed;
                result.Message = "Tỉnh / thành phố đã tồn tại!";
                return result;
            }
            try
            {
                await _cityRepository.InsertAsync(city);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        public async Task<BaseResult> Delete(int cityId)
        {
            var rs = new BaseResult() { Result = Result.Success };
            if (cityId > 0)
            {
                var city = _cityRepository.Query()
                                          .Include(c => c.Districts)
                                          .FirstOrDefault(c => c.Id == cityId);
                if (city != null)
                {
                    city.IsDeleted = true;
                    var districts = _districtRepository.Query()
                                                  .Include(d => d.City)
                                                  .Where(d => d.ParentId == cityId)
                                                  .ToList();
                    foreach (District d in districts)
                    {
                        await _districtService.Delete(d.Id);
                    }
                    _cityRepository.Update(city);
                    try
                    {
                        _cityRepository.SaveChange();
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

        public async Task<BaseResult> ExportToXlsx(SmartTableParam param)
        {
            var rs = new BaseResult() { Result = Result.Success };

            SmartTableResult<CityItem> cities = Search(param).Result;
            List<CityItem> list = cities.Items.ToList();
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Quản lí tỉnh, thành phố");


            var style = worksheet.Rows[0].Style;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            worksheet.Columns[0].SetWidth(50, LengthUnit.Pixel);
            worksheet.Columns[1].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[2].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[3].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[4].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[5].SetWidth(150, LengthUnit.Pixel);

            worksheet.Cells["A1"].Value = "ID";
            worksheet.Cells["B1"].Value = "Tên Tỉnh/Thành Phố";
            worksheet.Cells["C1"].Value = "Mã";
            worksheet.Cells["D1"].Value = "Miền";
            worksheet.Cells["E1"].Value = "Ngày Tạo";
            worksheet.Cells["F1"].Value = "Ngày Cập Nhật";

            worksheet.Tables.Add("Table1", "A1:F" + (list.Count + 1).ToString(), true);


            for (int r = 1; r <= list.Count; r++)
            {
                var item = list[r - 1];
                worksheet.Cells[r, 0].Value = item.Id;
                worksheet.Cells[r, 1].Value = item.Name;
                worksheet.Cells[r, 2].Value = item.Code;
                worksheet.Cells[r, 3].Value = item.CityRealmStr;
                worksheet.Cells[r, 4].Value = item.CreatedTimeDisplay;
                worksheet.Cells[r, 5].Value = item.UpdatedTimeDisplay;
            }
            string fileName = "ExportFile.xlsx";
            workbook.Save(fileName);

            rs.Message = "Thành Công!";
            return rs;
        }
        #endregion
    }
}
