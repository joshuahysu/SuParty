using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using SuParty.Data;
using SuParty.Data.DataModel;
using SuParty.Data.DataModel.RealEstate.Enum;
using System.Security.Claims;


namespace SuParty.Pages.Product
{
    public class ShoppingCartModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        public List<ProductData> productList = new();
        public ShoppingCartModel(ApplicationDbContext dbContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }
  
        public async Task<IActionResult> OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 查詢使用者
            var user = _dbContext.UserDatas.Find(userId);

            // 新增一筆資料到 ShoppingCart
            

            return Page();
        }

        /// <summary>
        /// 搜尋功能
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetSearch(SearchRequest request)
        {
            //IQueryable<ProductData> query = _dbContext.ProductDatas;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = _dbContext.UserDatas.Find(userId).ShoppingCart;

            // 計算滿足條件的總數量
            int totalRecords = query.Count();

            // 返回結果（包含分頁資訊）
            return new JsonResult(new
            {
                data = query,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords // 如果需要支持全局篩選，可另行處理
            });
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

            public ProductEnum productEnum { get; set; }
            public int PageSize { get; internal set; }
            public int Page { get; internal set; }
        }
    }
}
