using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using System.Security.Claims;

namespace SuParty.Pages.User
{
    public class EditUserDataModel : PageModel
    {
        [BindProperty]
        public UserData UserData { get; set; } // �j�w UserData �ҫ�

        private readonly ApplicationDbContext _dbContext;

        public EditUserDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void OnGet()
        {
            // �T�O�b�����[�������� User ���O null
            if (UserData == null)
            {
                UserData = new UserData(); // ��l�� User ����
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                UserData.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //�۰�insert or update
                _dbContext.UserDatas.Update(UserData);
                await _dbContext.SaveChangesAsync();
                return RedirectToPage("/Success");
            }
            return Page();
        }
    }
}
