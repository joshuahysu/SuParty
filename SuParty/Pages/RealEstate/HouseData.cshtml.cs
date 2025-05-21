using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using SuParty.Data.DataModel.RealEstate;
using SuParty.Service.Qrcode;
using SuParty.Service.SendEmail;
using System.Security.Claims;
using System.Text.Json;

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
        public async Task<IActionResult> OnPostTraceRealEstates(string id,int loveScore)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // 查詢使用者
                RealEstateUserData? user = await _dbContext.UserDatas.FindAsync(userId);
                // 新增一筆資料到 追蹤列表
                user.TraceRealEstates.Add(new TrackingObject() { LoveScore = loveScore, Id = id });

                //
                //EmailHelper.SendEmail(
                //            smtpHost: "smtp.gmail.com",
                //    smtpPort: 587,
                //    smtpUser: "your_email@gmail.com",
                //    smtpPassword: "your_app_password", // Gmail 建議使用 App 密碼
                //    fromEmail: "your_email@gmail.com",
                //    toEmail: "recipient@example.com",
                //    subject: "Hello!",
                //    body: "This is a test email from a reusable function.",
                //    enableSsl: true
                //);


                // 保存變更到資料庫
                await _dbContext.SaveChangesAsync();                

            }
            return new JsonResult(new { success = true, message = "Add success" });
        }


        public async Task<IActionResult> OnPostTracking(string id, bool tracking,int loveScore)
        {
            if (User.Identity.IsAuthenticated)
            {
                // 取得登入者的帳號（用戶名或電子郵件）
                string username = User.Identity.Name;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var dbtracking = await _dbContext.Trackings.FindAsync(userId);
                if (dbtracking == null)
                {
                    dbtracking = new Tracking();
                    dbtracking.Id = userId;
                    dbtracking.LoveScore = loveScore;
                    _dbContext.Trackings.Add(dbtracking);
                }
                if (tracking)
                {
                    //不存在才新增
                    if (!dbtracking.TrackingList.Any(t => t.Id == id))
                    {
                        dbtracking.TrackingList.Add(new TrackingObject() { LoveScore = loveScore, Id = id });
                    }
                }
                else
                    dbtracking.TrackingList.Remove(new TrackingObject() { LoveScore = loveScore, Id = id });
                if (loveScore>79) {
                    EmailHelper.SendEmail(
                                smtpHost: "smtp.gmail.com",
                        smtpPort: 587,
                        smtpUser: "your_email@gmail.com",
                        smtpPassword: "your_app_password", // Gmail 建議使用 App 密碼
                        fromEmail: "your_email@gmail.com",
                        toEmail: "recipient@example.com",
                        subject: "Hello!",
                        body: "This is a test email from a reusable function.",
                        enableSsl: true
                    );
                }

                await _dbContext.SaveChangesAsync();
                var successResponse = new { success = true, message = "追蹤狀態更新成功。" };
                return Content(JsonSerializer.Serialize(successResponse), "application/json");
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }
        }
    }
}