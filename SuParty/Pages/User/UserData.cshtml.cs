using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
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
        public IActionResult OnGet()
        {

            if (User.Identity.IsAuthenticated)
            {
                // 取得登入者的帳號（用戶名或電子郵件）
                string username = User.Identity.Name;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // 使用登入者帳號做其他處理
                Message =$"Logged in as: {username}";

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
    }
}
