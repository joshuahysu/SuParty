using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using System.Reflection;

namespace SuParty.Pages.User
{
    public class UserDataModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public UserDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // �Ω�ǻ���e�ݪ����
        public List<Product> Products { get; set; }
        public string Message { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Message = "�w��A�z�w�n�J�I";

                // �q��Ʈw�����o�Ҧ����~
                //Products = _dbContext.Products.ToList();
            }
            else
            {
                Message = "�z�|���n�J�A�N���w�V��n�J�����C";
                //return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}
