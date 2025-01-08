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
                // ���o�n�J�̪��b���]�Τ�W�ιq�l�l��^
                string username = User.Identity.Name;
                // �ϥεn�J�̱b������L�B�z  

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
