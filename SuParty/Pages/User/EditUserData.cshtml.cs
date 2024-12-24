using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using System.Security.Claims;

namespace SuParty.Pages.User
{
    public class EditUserDataModel : PageModel
    {
        [BindProperty]
        public UserData UserData { get; set; } // 綁定 UserData 模型

        private readonly ApplicationDbContext _dbContext;

        public EditUserDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void OnGet()
        {
            // 確保在首次加載頁面時 User 不是 null
            if (UserData == null)
            {
                UserData = new UserData(); // 初始化 User 物件
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                UserData.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //自動insert or update
                _dbContext.UserDatas.Update(UserData);
                await _dbContext.SaveChangesAsync();
                return RedirectToPage("/Success");
            }
            return Page();
        }
    }
}
