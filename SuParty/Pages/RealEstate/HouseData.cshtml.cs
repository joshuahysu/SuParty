using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using SuParty.Data.DataModel.RealEstate;
using System.Security.Claims;

namespace SuParty.Pages.RealEstate
{
    public class HouseDataModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public HouseData? HouseData { get; set; } = new HouseData();

        public HouseDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult OnGet(string id)
        {
            HouseData = _dbContext.HouseDatas.Find(id);
            if (User.Identity.IsAuthenticated)
            {
                //有會員也許修改價格

            }
            else
            {
  
            }
            
            return Page();
        }
        public IActionResult OnPostTraceRealEstates(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // 查詢使用者
                UserData? user = _dbContext.UserDatas.Find(userId);

                // 新增一筆資料到 追蹤列表
                user.TraceRealEstates.Add(id);

                // 保存變更到資料庫
                _dbContext.SaveChanges();                

            }
            return new JsonResult(new { success = true, message = "Add success" });
        }        
    }
}