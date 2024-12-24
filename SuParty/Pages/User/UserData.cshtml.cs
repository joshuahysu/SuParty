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
        public string Message { get; set; }

        public IActionResult OnGet()
        {

            if (User.Identity.IsAuthenticated)
            {
                // ���o�n�J�̪��b���]�Τ�W�ιq�l�l��^
                string username = User.Identity.Name;
                // �ϥεn�J�̱b������L�B�z
                Message=$"Logged in as: {username}";
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
