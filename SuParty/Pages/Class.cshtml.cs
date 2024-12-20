using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace SuParty.Pages
{
    public class ClassModel : PageModel
    {
        public string Message { get; private set; }
        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Message = "歡迎，您已登入！";
            }
            else
            {
                Message = "您尚未登入，將重定向到登入頁面。";
                // 可手動重定向至登入頁面
                RedirectToPage("/Account/Login");
            }
        }
    }
}
