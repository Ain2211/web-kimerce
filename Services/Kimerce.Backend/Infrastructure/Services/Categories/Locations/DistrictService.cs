using GemBox.Spreadsheet;
using Kimerce.Backend.Domain.Categories;
using Kimerce.Backend.Domain.Locations;
using Kimerce.Backend.Dto;
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
    public interface IDistrictService
    {
        Task<SmartTableResult<DistrictItem>> Search(SmartTableParam param);
        Task<BaseResult> CreateOrUpdate(DistrictModel model, int updateBy = 0, string updateByUserName = "");
        DistrictModel Get(int id);
        IQueryable<WardItem> GetChildren(int id);
        Task<BaseResult> Delete(int id);
        Task<BaseResult> ExportToXlsx(SmartTableParam param);
    }

    public class DistrictService : IDistrictService
    {
        private readonly IRepository<District> _districtRepository;
        private readonly IRepository<Ward> _wardRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IWardService _wardService;
        public DistrictService(IRepository<District> districtRepository, IRepository<Ward> wardRepository, 
                                                IRepository<City> cityRepository, IWardService wardService)
        {
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
            _cityRepository = cityRepository;
            _wardService = wardService;
        }

        /// <summary>
        /// Tìm kiếm danh sách DC
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<SmartTableResult<DistrictItem>> Search(SmartTableParam param)
        {
            var query = _districtRepository.Query()
                            .Include(d => d.City)
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
                                          || x.City.Name.ToLower().Contains(keyword)
                                          || x.City.UnsignName.Contains(keyword));
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

                if (search.Id > 0 && search.Id != null)
                {
                    int id = search.Id;
                    query = query.Where(x => x.Id == id);
                }
            }
            param.Sort = new Sort() { Predicate = "DisplayOrder", Reverse = false };
            var gridData = query.ToSmartTableResult(param, x => x.ToItem());
            return gridData;
        }

        #region CRUD
        public DistrictModel Get(int id)
        {
            return id > 0 ? _districtRepository.Query()
                                               .Include(d => d.City)
                                               .Include(d => d.Wards)
                                               .FirstOrDefault(d => d.Id == id).ToModel() : new DistrictModel();
        }

        public IQueryable<WardItem> GetChildren(int id)
        {
            return _wardRepository.Query().Where(x => x.ParentId == id).Select(x => x.ToItem());

        }

        public async Task<BaseResult> CreateOrUpdate(DistrictModel model, int updateBy = 0, string updateByUserName = "")
        {
            var district = model.ToDistrict();

            //Cập nhật thông tin chung của thực thể
            district = district.UpdateCommonInt(updateBy, updateByUserName);

            if (district.Id > 0)
            {
                //Cập nhật
                return await Update(district);
            }
            else
            {
                //Thêm mới
                return await Create(district);
            }
        }

        private async Task<BaseResult> Update(District district, int updateBy = 0, string updateByUserName = "")
        {
            var result = new BaseResult() { Result = Result.Success };
            var districtForUpdate = _districtRepository.Query().FirstOrDefault(d => d.Id == district.Id);
            if (districtForUpdate == null || district.Id <= 0)
            {
                result.Result = Result.Failed;
                result.Message = "Không tìm thấy quận yêu cầu!";
                return result;
            }
            else
            {
                var city = _cityRepository.Query()
                            .AsNoTracking()
                            .FirstOrDefault(c => c.Id == district.ParentId);
                var exists = _districtRepository.Query()
                            .Include(d => d.City)
                            .Any(c => !c.IsDeleted && c.City.Name == city.Name && c.Name == district.Name && c.Id != districtForUpdate.Id);
                if (exists)
                {
                    result.Result = Result.Failed;
                    result.Message = "Quận/Huyện đã tồn tại!";
                    return result;
                }
                district.CityRealm = city.CityRealm;
                district.Name = district.Name.Trim();
                district.UnsignName = !string.IsNullOrEmpty(district.Name) ? district.Name.Unsigned() : "";
            }
            try
            {
                districtForUpdate = district.ToDistrict(districtForUpdate);

                //Cập nhật thông tin chung của thực thể
                districtForUpdate = districtForUpdate.UpdateCommonInt(updateBy, updateByUserName);

                await _districtRepository.UpdateAsync(districtForUpdate);
                var wards = _wardRepository.Query()
                                  .Include(d => d.District)
                                  .Where(d => d.ParentId == districtForUpdate.Id)
                                  .ToList();
                foreach (Ward w in wards)
                {
                    WardModel ward = w.ToModel();
                    ward.CityRealm = districtForUpdate.CityRealm;
                    await _wardService.CreateOrUpdate(ward);
                }
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        private async Task<BaseResult> Create(District district)
        {
            var result = new BaseResult();
            var city = _cityRepository.Query()
                            .Include(c => c.Districts)
                            .AsNoTracking()
                            .FirstOrDefault(c => c.Id == district.ParentId);
            district.CityRealm = city.CityRealm;
            district.Name = district.Name.Trim();
            district.UnsignName = !string.IsNullOrEmpty(district.Name) ? district.Name.Unsigned() : "";
            
            var exists = _districtRepository.Query()
                            .Include(d => d.City)
                            .Any(c =>c.City.Name == city.Name && c.Name == district.Name);
            if (exists)
            {
                result.Result = Result.Failed;
                result.Message = "Quận/Huyện đã tồn tại!";
                return result;
            }
            try
            {
                await _districtRepository.InsertAsync(district);
            }
            catch (Exception e)
            {
                result.Result = Result.SystemError;
                result.Message = e.ToString();
            }
            return result;
        }

        public async Task<BaseResult> Delete(int districtId)
        {
            var rs = new BaseResult() { Result = Result.Success };
            if (districtId > 0)
            {
                var district = _districtRepository.Query()
                                                  .Include(d => d.Wards)
                                                  .FirstOrDefault(d => d.Id == districtId);
                
                if (district != null)
                {
                    district.IsDeleted = true;
                    var wards = _wardRepository.Query()
                                                  .Include(w => w.District)
                                                  .Where(w => w.ParentId == districtId)
                                                  .ToList();
                    foreach (Ward w in wards)
                    {
                        await _wardService.Delete(w.Id);
                    }
                    
                    _districtRepository.Update(district);
                    try
                    {
                        _districtRepository.SaveChange();
                    }
                    catch (Exception ex)
                    {
                        rs.Result = Result.SystemError;
                        rs.Message = ex.ToString();
                    }
                }
                else
                {
                    rs.Message = "Không tìm thấy huyện / quận yêu cầu!";
                    rs.Result = Result.Failed;
                }

            }
            else
            {
                rs.Message = "Mã huyện / quận không hợp lệ!";
                rs.Result = Result.Failed;
            }
            return rs;
        }

        public async Task<BaseResult> ExportToXlsx(SmartTableParam param)
        {
            var rs = new BaseResult() { Result = Result.Success };

            SmartTableResult<DistrictItem> districts = Search(param).Result;
            List<DistrictItem> list = districts.Items.ToList();
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Quản lí huyện, quận");


            var style = worksheet.Rows[0].Style;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            worksheet.Columns[6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

            worksheet.Columns[0].SetWidth(50, LengthUnit.Pixel);
            worksheet.Columns[1].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[2].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[4].SetWidth(100, LengthUnit.Pixel);
            worksheet.Columns[5].SetWidth(150, LengthUnit.Pixel);
            worksheet.Columns[6].SetWidth(150, LengthUnit.Pixel);

            worksheet.Cells["A1"].Value = "ID";
            worksheet.Cells["B1"].Value = "Tên Huyện/Quận";
            worksheet.Cells["C1"].Value = "Tỉnh/Thành Phố";
            worksheet.Cells["D1"].Value = "Mã";
            worksheet.Cells["E1"].Value = "Miền";
            worksheet.Cells["F1"].Value = "Ngày Tạo";
            worksheet.Cells["G1"].Value = "Ngày Cập Nhật";

            worksheet.Tables.Add("Table1", "A1:G" + (list.Count + 1).ToString(), true);


            for (int r = 1; r <= list.Count; r++)
            {
                var item = list[r - 1];
                worksheet.Cells[r, 0].Value = item.Id;
                worksheet.Cells[r, 1].Value = item.Name;
                worksheet.Cells[r, 2].Value = item.City.Name;
                worksheet.Cells[r, 3].Value = item.Code;
                worksheet.Cells[r, 4].Value = item.CityRealmStr;
                worksheet.Cells[r, 5].Value = item.CreatedTimeDisplay;
                worksheet.Cells[r, 6].Value = item.UpdatedTimeDisplay;
            }
            string fileName = "ExportFile.xlsx";
            workbook.Save(fileName);

            rs.Message = "Thành Công!";
            return rs;
        }
        #endregion
    }

}
