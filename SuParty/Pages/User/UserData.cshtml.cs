using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using System.Security.Claims;
using System.Text.Json;

namespace SuParty.Pages.User
{
    public class UserDataModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public UserDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // 用於傳遞到前端的資料
        public string Message { get; set; }
        public RealEstateUserData? UserData { get; set; }=new RealEstateUserData();
        public async Task<IActionResult> OnGet(string userId)
        {
            if (User.Identity.IsAuthenticated)
            {
                // 取得登入者的帳號（用戶名或電子郵件）
                string username = User.Identity.Name;
                if(String.IsNullOrEmpty(userId))
                     userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // 使用登入者帳號做其他處理
                //Message =$"Logged in as: {username}";
                //目前全公開
                UserData = await _dbContext.UserDatas.FindAsync(userId);

                return Page();
            }
            else
            {
                Message = "您尚未登入，將重定向到登入頁面。";
                return Redirect("/Identity/Account/Login");
            }

        }

        public async Task<IActionResult> OnPostTracking(string id,bool tracking,int loveScore)
        {
            if (User.Identity.IsAuthenticated)
            {
                // 取得登入者的帳號（用戶名或電子郵件）
                string username = User.Identity.Name;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var dbtracking = await _dbContext.TrackingUsers.FindAsync(userId);
                if (dbtracking == null)
                {
                    dbtracking=new Tracking();
                    dbtracking.Id = userId;
                    dbtracking.TrackingList.Add(new TrackingObject()
                    {
                        LoveScore = loveScore,
                        Id = id
                    });
                    _dbContext.TrackingUsers.Add(dbtracking);
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
                await _dbContext.SaveChangesAsync();
                var successResponse = new { success = true, message = "追蹤狀態更新成功。" };
                return Content(JsonSerializer.Serialize(successResponse), "application/json");
            }
            else
            {
                Message = "您尚未登入，將重定向到登入頁面。";
                return Redirect("/Identity/Account/Login");
            }
        }        
    }
}