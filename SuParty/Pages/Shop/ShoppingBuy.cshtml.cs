using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using System.Security.Claims;

namespace SuParty.Pages.Shop
{
    public class ShoppingBuyModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        public List<ProductData> ShoppingCart;

        public ShoppingBuyModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Data.DataModel.RealEstateUserData? user = _dbContext.UserDatas.Find(userId);

                 ShoppingCart = user.ShoppingCart;

            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var transactionId = Request.Form["transaction_id"];
            var userId = Request.Form["user_id"];
            var amount = decimal.Parse(Request.Form["amount"]);

            // 確保交易與分潤在同一個資料庫交易中
            await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                //await SaveTransactionAsync(transactionId, userId, amount);
                //await _profitService.RunProfitSharingAsync(userId, amount);

                await dbTransaction.CommitAsync(); // 提交交易
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync(); // 回滾整個交易
                //_logger.LogError(ex, "交易或分潤處理失敗");
                return RedirectToPage("PaymentFailed"); // 或顯示錯誤頁
            }

            return RedirectToPage("PaymentSuccess");
        }

    }
}
