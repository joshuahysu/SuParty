using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SuParty.Data;
using SuParty.Data.DataModel.RealEstate;
using SuParty.Data.DataModel.RealEstate.Enum;

namespace SuParty.Pages.RealEstate
{
    public class HouseListModel : PageModel
    {
        public List<HouseData> productList = new();

        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;

        public HouseListModel(ApplicationDbContext dbContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }
        public async Task<IActionResult> OnGet()
        {
            //要有快取避免天天搜尋
            // 嘗試從快取中取得資料
            if (!_cache.TryGetValue("HouseListModel", out string data))
            {
                // 如果快取不存在，則計算資料
                data = "這是新的資料 " + DateTime.Now.ToString();

                // 設置快取（持續 5 分鐘）
                _cache.Set("HouseListModel", data, TimeSpan.FromMinutes(5));
            }

            return Page();
        }

        public async Task<IActionResult> OnGetSearch(SearchRequest request)
        {
            IQueryable<HouseData> query = _dbContext.HouseDatas;

            // 建立多欄位搜尋條件
            query = query = query.Where(p =>
            (request.MinPrice != null ? p.Price >= request.MinPrice.Value : true) &&
            (request.MaxPrice != null ? p.Price <= request.MaxPrice.Value : true) &&
            (request.MinPricePerPing != null ? p.PricePerPing >= request.MinPricePerPing.Value : true) &&
            (request.MaxPricePerPing != null ? p.PricePerPing <= request.MaxPricePerPing.Value : true) &&
            (request.MinRoomCount > 0 ? p.RoomCount >= request.MinRoomCount : true) && // 0 代表不篩選
            (request.MaxRoomCount > 0 ? p.RoomCount <= request.MaxRoomCount : true) &&
            (request.MinRestroomCount > 0 ? p.RestroomCount >= request.MinRestroomCount : true) &&
            (request.MaxRestroomCount > 0 ? p.RestroomCount <= request.MaxRestroomCount : true) &&
            (request.MinLivingRoomCount > 0 ? p.LivingRoomCount >= request.MinLivingRoomCount : true) &&
            (request.MaxLivingRoomCount > 0 ? p.LivingRoomCount <= request.MaxLivingRoomCount : true) &&
            (request.MinParkingSpaceCount > 0 ? p.ParkingSpaceCount >= request.MinParkingSpaceCount : true) &&
            (request.MaxParkingSpaceCount > 0 ? p.ParkingSpaceCount <= request.MaxParkingSpaceCount : true) &&
            (request.MinFloor > 0 ? p.Floor >= request.MinFloor : true) &&
            (request.MaxFloor > 0 ? p.Floor <= request.MaxFloor : true) &&
            (request.City+1 > 0 ? p.City == request.City : true) // 0 代表不篩選
);


            // 計算滿足條件的總數量
            int totalRecords = query.Count();

            // 應用分頁
            int pageSize = request.PageSize > 0 ? request.PageSize : 10;
            int page = request.Page > 0 ? request.Page - 1 : 0;

            // 直接在查詢中使用 Skip 和 Take 完成分頁
            var pagedData = await query
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();           

            // 返回結果（包含分頁資訊）
            return new JsonResult(new
            {
                data = pagedData,    
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords // 如果需要支持全局篩選，可另行處理
            });
        }
    }
    public class SearchRequest
    {
        public decimal? MinPrice { get; set; }   
        public decimal? MaxPrice { get; set; } 

        public float? MinPricePerPing { get; set; }   
        public float? MaxPricePerPing { get; set; } 

        public float? MinSpace { get; set; }   
        public float? MaxSpace { get; set; }   
        public int MinRoomCount { get; set; }

        public int MaxRoomCount { get; set; }

        public int MaxRestroomCount { get; set; }
        public int MinRestroomCount { get; set; } 

        public int MaxLivingRoomCount { get; set; }
        public int MinLivingRoomCount { get; set; }

        public int MaxParkingSpaceCount { get; set; }
        public int MinParkingSpaceCount { get; set; }
        public int MaxFloor { get; set; }
        public int MinFloor { get; set; }

        public CityEnum City { get; set; }
        public int PageSize { get; internal set; }
        public int Page { get; internal set; }
    }
}
