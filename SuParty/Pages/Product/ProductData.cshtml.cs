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
                // ���o�n�J�̪��b���]�Τ�W�ιq�l�l��^
                string username = User.Identity.Name;
                // �ϥεn�J�̱b������L�B�z  
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

                // �d�ߨϥΪ�
                var user = _dbContext.UserDatas.Find(userId);

                // �s�W�@����ƨ� ShoppingCart
                user.ShoppingCart.Add(Id);

                // �O�s�ܧ���Ʈw
                _dbContext.SaveChanges();                

            }
            return new JsonResult(new { success = true, message = "Buy success" });
        }        
    }
}