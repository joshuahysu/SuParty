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

        /// <summary>
        /// ���o�C��
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGet()
        {
            //�n���֨��קK�Ѥѷj�M
            // ���ձq�֨������o���
            if (!_cache.TryGetValue("HouseListModel", out IQueryable<HouseData> data))
            {
                // �p�G�֨����s�b�A�h�����
                data = _dbContext.HouseDatas;
                productList = data.ToList();
                // �]�m�֨��]���� 5 �����^
                _cache.Set("HouseListModel", data, TimeSpan.FromMinutes(5));
            }

            return Page();
        }

        /// <summary>
        /// �j�M�\��
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetSearch(SearchRequest request)
        {
            IQueryable<HouseData> query = _dbContext.HouseDatas;
            // �إߦh���j�M����
            query = query.Where(p =>
         (!request.MinPrice.HasValue || p.Price >= request.MinPrice.Value) &&
         (!request.MaxPrice.HasValue || p.Price <= request.MaxPrice.Value) &&
         (!request.MinPricePerPing.HasValue || p.PricePerPing >= request.MinPricePerPing.Value) &&
         (!request.MaxPricePerPing.HasValue || p.PricePerPing <= request.MaxPricePerPing.Value) &&
         (request.MinRoomCount <= 0 || p.RoomCount >= request.MinRoomCount) &&
         (request.MaxRoomCount <= 0 || p.RoomCount <= request.MaxRoomCount) &&
         (request.MinRestroomCount <= 0 || p.RestroomCount >= request.MinRestroomCount) &&
         (request.MaxRestroomCount <= 0 || p.RestroomCount <= request.MaxRestroomCount) &&
         (request.MinLivingRoomCount <= 0 || p.LivingRoomCount >= request.MinLivingRoomCount) &&
         (request.MaxLivingRoomCount <= 0 || p.LivingRoomCount <= request.MaxLivingRoomCount) &&
         (request.MinParkingSpaceCount <= 0 || p.ParkingSpaceCount >= request.MinParkingSpaceCount) &&
         (request.MaxParkingSpaceCount <= 0 || p.ParkingSpaceCount <= request.MaxParkingSpaceCount) &&
         (request.MinFloor <= 0 || p.Floor >= request.MinFloor) &&
         (request.MaxFloor <= 0 || p.Floor <= request.MaxFloor) &&
         (request.City <= 0 || p.City == request.City)
     );


            // �p�⺡�������`�ƶq
            int totalRecords = query.Count();

            // ���Τ���
            int pageSize = request.PageSize > 0 ? request.PageSize : 10;
            int page = request.Page > 0 ? request.Page - 1 : 0;

            // �����b�d�ߤ��ϥ� Skip �M Take ��������
            var pagedData = await query
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();           

            // ��^���G�]�]�t������T�^
            return new JsonResult(new
            {
                data = pagedData,    
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords // �p�G�ݭn��������z��A�i�t��B�z
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
