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
        public List<Product> Products { get; set; }
        public string Message { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Message = "歡迎，您已登入！";

                // 從資料庫中取得所有產品
                //Products = _dbContext.Products.ToList();
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
