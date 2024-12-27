using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using System.Security.Claims;

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
        public UserData? UserData { get; set; }=new UserData();
        public IActionResult OnGet(string userId)
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
                UserData = _dbContext.UserDatas.Find(userId);

                return Page();
            }
            else
            {
                Message = "您尚未登入，將重定向到登入頁面。";
                //return RedirectToPage("/Account/Login");
            }
            return null;

        }

        public IActionResult OnPostTracking(string id,bool tracking)
        {
            if (User.Identity.IsAuthenticated)
            {
                // 取得登入者的帳號（用戶名或電子郵件）
                string username = User.Identity.Name;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var dbtracking = _dbContext.Trackings.Find(userId);
                if (tracking)
                {
                    //不存在才新增
                    if (!dbtracking.TrackingList.Any(t => t == id))
                    {
                        dbtracking.TrackingList.Add(id);
                    }
                }
                else
                    dbtracking.TrackingList.Remove(id);
                _dbContext.SaveChanges();
                return Page();
            }
            else
            {
                Message = "您尚未登入，將重定向到登入頁面。";
                //return RedirectToPage("/Account/Login");
            }

            return Page();
        }
        
    }
}
