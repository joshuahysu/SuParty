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
            // ���Ҽƾ�
            if (!ModelState.IsValid)
            {
                // ���ҥ��ѡA��^���~�H��
                return Page(); // ��^��e�����H��ܿ��~
            }
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // �d�ߨϥΪ�
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
                // �d�ߨϥΪ�
                var user = _dbContext.UserDatas.Find(userId);
                user.Store.Remove(id);
                _dbContext.SaveChanges();
                return new JsonResult(new { success = true, message = "success" });
            }
            return new JsonResult(new { success = false, message = "error" });
        }
    }
}