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

            // �T�O����P����b�P�@�Ӹ�Ʈw�����
            await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                //await SaveTransactionAsync(transactionId, userId, amount);
                //await _profitService.RunProfitSharingAsync(userId, amount);

                await dbTransaction.CommitAsync(); // ������
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync(); // �^�u��ӥ��
                //_logger.LogError(ex, "����Τ���B�z����");
                return RedirectToPage("PaymentFailed"); // ����ܿ��~��
            }

            return RedirectToPage("PaymentSuccess");
        }

    }
}
