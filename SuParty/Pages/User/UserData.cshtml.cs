using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using System.Security.Claims;

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
        public UserData? UserData { get; set; }=new UserData();
        public IActionResult OnGet()
        {

            if (User.Identity.IsAuthenticated)
            {
                // ���o�n�J�̪��b���]�Τ�W�ιq�l�l��^
                string username = User.Identity.Name;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // �ϥεn�J�̱b������L�B�z
                Message =$"Logged in as: {username}";

                UserData = _dbContext.UserDatas.Find(userId);

                return Page();
            }
            else
            {
                Message = "�z�|���n�J�A�N���w�V��n�J�����C";
                //return RedirectToPage("/Account/Login");
            }
            return null;

        }
    }
}
