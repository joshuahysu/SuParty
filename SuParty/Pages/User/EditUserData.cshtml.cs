using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using System;
using System.Reflection;

namespace SuParty.Pages.User
{
    public class EditUserDataModel : PageModel
    {
        [BindProperty]
        public UserData User { get; set; } // �j�w UserData �ҫ�

        private readonly ApplicationDbContext _dbContext;

        public EditUserDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void OnGet()
        {
            // �T�O�b�����[�������� User ���O null
            if (User == null)
            {
                User = new UserData(); // ��l�� User ����
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // �p�G���Ʊ�� Email ���i�����ҡA�i�H�����������~
            ModelState.Remove(nameof(User.Email));
            if (ModelState.IsValid)
            {
                _dbContext.UserDatas.Add(User);
                await _dbContext.SaveChangesAsync();
                return RedirectToPage("/Success");
            }
            return Page();
        }
    }
}
