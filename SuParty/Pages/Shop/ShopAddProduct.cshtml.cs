using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data.DataModel;
using SuParty.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace SuParty.Pages.Shop
{
    public class ShopAddProductModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        [BindProperty]
        public ProductData ProductData { get; set; }

        public ShopAddProductModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPostAddProduct(string id)
        {
            // 驗證數據
            if (!ModelState.IsValid)
            {
                // 驗證失敗，返回錯誤信息
                return Page(); // 返回當前頁面以顯示錯誤
            }
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // 查詢使用者
                var user = _dbContext.UserDatas.Find(userId);
                user.Store.Add(id);
                _dbContext.ProductDatas.Add(ProductData);
                _dbContext.SaveChanges();

                return new JsonResult(new { success = true, message = "success" });
            }
            else
            {
              
            }
            return new JsonResult(new { success = true, message = "success" });
        }
        public IActionResult OnPostDeleteProduct(string id) {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // 查詢使用者
                var user = _dbContext.UserDatas.Find(userId);
                user.Store.Remove(id);
                _dbContext.SaveChanges();
                return new JsonResult(new { success = true, message = "success" });
            }
            return new JsonResult(new { success = false, message = "error" });
        }
    }
}