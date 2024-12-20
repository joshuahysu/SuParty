using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;

namespace SuParty.Pages
{
    public class ClassModel : PageModel
    {
        public string Message { get; private set; }
        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Message = "�w��A�z�w�n�J�I";
            }
            else
            {
                Message = "�z�|���n�J�A�N���w�V��n�J�����C";
                // �i��ʭ��w�V�ܵn�J����
                RedirectToPage("/Account/Login");
            }
        }
    }
}
