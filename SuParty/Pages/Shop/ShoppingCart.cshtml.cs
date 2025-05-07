using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using System.Security.Claims;

namespace SuParty.Pages.Shop
{
    public class ShoppingCartModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        public List<string> ShoppingCart;

        public ShoppingCartModel(ApplicationDbContext dbContext)
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

        public void OnPostAddProduct(string Id)
        {
        }
    }
}
