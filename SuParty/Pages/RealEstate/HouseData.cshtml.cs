using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using SuParty.Data.DataModel.RealEstate;
using SuParty.Service.Qrcode;
using System.Security.Claims;

namespace SuParty.Pages.RealEstate
{
    public class HouseDataModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public HouseData? HouseData { get; set; } = new HouseData();

        public RealEstateUserData? SaleUser { get; set; } = new ();
        public string Base64QRCode { get; set; } = "";

        public string UserID { get; set; } = "";
        public HouseDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnGet(string id,string promoter)
        {
            HouseData = await _dbContext.HouseDatas.FindAsync(id);
            SaleUser =await _dbContext.UserDatas.FindAsync(HouseData.SalesId);
            if(SaleUser!=null)
            Base64QRCode = Qrcode.CreateQrcode(SaleUser.Line_Url);//Line的Qrcode
            if (User.Identity.IsAuthenticated)
            {
                UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            else
            {
  
            }
            
            return Page();
        }
        public async Task<IActionResult> OnPostTraceRealEstates(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // 查詢使用者
                RealEstateUserData? user = await _dbContext.UserDatas.FindAsync(userId);

                // 新增一筆資料到 追蹤列表
                user.TraceRealEstates.Add(id);

                // 保存變更到資料庫
                await _dbContext.SaveChangesAsync();                

            }
            return new JsonResult(new { success = true, message = "Add success" });
        }        
    }
}