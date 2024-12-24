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
        public UserData User { get; set; } // 綁定 UserData 模型

        private readonly ApplicationDbContext _dbContext;

        public EditUserDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void OnGet()
        {
            // 確保在首次加載頁面時 User 不是 null
            if (User == null)
            {
                User = new UserData(); // 初始化 User 物件
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // 如果不希望對 Email 欄位進行驗證，可以移除相關錯誤
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
