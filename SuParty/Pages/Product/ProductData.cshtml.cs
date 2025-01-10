using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuParty.Data;
using SuParty.Data.DataModel;
using System.Security.Claims;

namespace SuParty.Pages.Product
{
    public class ProductDataModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductData? ProductData { get; set; } = new ProductData();

        public ProductDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult OnGet(string Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                // 取得登入者的帳號（用戶名或電子郵件）
                string username = User.Identity.Name;
                // 使用登入者帳號做其他處理  
            }
            else
            {
                ProductData = _dbContext.ProductDatas.Find(Id);
            }
            return Page();
        }
        public IActionResult OnPostAddShoppingCart(string Id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // 查詢使用者
                var user = _dbContext.UserDatas.Find(userId);

                // 新增一筆資料到 ShoppingCart
                user.ShoppingCart.Add(Id);

                // 保存變更到資料庫
                _dbContext.SaveChanges();                

            }
            return new JsonResult(new { success = true, message = "Buy success" });
        }        
    }
}