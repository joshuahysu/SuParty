using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using SuParty.Service.Referrer;
using System.Security.Claims;

namespace SuParty.Pages.User
{
    public class EditUserDataModel : PageModel
    {
        [BindProperty]
        public RealEstateUserData UserData { get; set; } // 綁定 UserData 模型

        private readonly ApplicationDbContext _dbContext;

        public EditUserDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> OnGet()
        {
            UserData = _dbContext.UserDatas.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));
            // 確保在首次加載頁面時 User 不是 null
            if (UserData == null)
            {
                UserData = new RealEstateUserData(); // 初始化 User 物件
            }
            return Page();
        }

        /// <summary>
        /// 更新個人資料
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    UserData.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    //自動insert or update
                    _dbContext.UserDatas.Update(UserData);

                    //需要更新直銷部分
                    ReferrerMember referrerMember = _dbContext.ReferrerMembers.Find(UserData.Id);

                    if (referrerMember == null)
                    {
                        referrerMember = new ReferrerMember() {
                            Id=UserData.Id,
                        }; // 初始化 referrerMember 物件
                    }
                    referrerMember.Name = UserData.Name;
                    _dbContext.ReferrerMembers.Update(referrerMember);

                    await _dbContext.SaveChangesAsync();
                    return RedirectToPage("/User/UserData");
                }
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }
            return Page();
        }
    }
}