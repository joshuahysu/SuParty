using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public IActionResult OnGet(string id)
        {
            ProductData = _dbContext.ProductDatas.Find(id);
            if (User.Identity.IsAuthenticated)
            {
                //有會員也許修改價格

            }
            else
            {
  
            }
            
            return Page();
        }
        public IActionResult OnPostAddShoppingCart(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // 查詢使用者
                var user = _dbContext.UserDatas.Find(userId);

                // 新增一筆資料到 ShoppingCart
                user.ShoppingCart.Add(new ProductData { Id = id });

                // 保存變更到資料庫
                _dbContext.SaveChanges();                

            }
            return new JsonResult(new { success = true, message = "Add success" });
        }        
    }
}