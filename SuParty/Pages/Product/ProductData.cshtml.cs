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

        public IActionResult OnGet(string ProductId)
        {
            if (User.Identity.IsAuthenticated)
            {
                // 取得登入者的帳號（用戶名或電子郵件）
                string username = User.Identity.Name;
                // 使用登入者帳號做其他處理  

                return Page();
            }
            else
            {
                ProductData = _dbContext.ProductDatas.Find(ProductId);

            }
            return Page();
        }
        public IActionResult OnPostBuy(string ProductId)
        {
           return new JsonResult(new { success = false, message = "Buy success" });
        }        
    }
}
