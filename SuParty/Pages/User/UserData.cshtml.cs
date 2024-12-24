using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using System.Reflection;

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

        public IActionResult OnGet()
        {

            if (User.Identity.IsAuthenticated)
            {
                // 取得登入者的帳號（用戶名或電子郵件）
                string username = User.Identity.Name;
                // 使用登入者帳號做其他處理
                Message=$"Logged in as: {username}";
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
