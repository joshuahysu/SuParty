using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
            if (User.Identity.IsAuthenticated)
            {
                UserData = _dbContext.UserDatas.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));
                // 確保在首次加載頁面時 User 不是 null
                if (UserData == null)
                {
                UserData = new RealEstateUserData(); // 初始化 User 物件
                }
            }
            else
            {
                return Redirect("/Identity/Account/Login");
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
                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    UserData.Id = userId;

                    // 先查詢資料庫中是否已存在該使用者資料
                    var existingUserData = _dbContext.UserDatas.AsNoTracking().FirstOrDefault(h => h.Id == userId);

                    if (existingUserData == null)
                    {
                        _dbContext.UserDatas.Add(UserData); // 新增
                    }
                    else
                    {
                        _dbContext.UserDatas.Update(UserData);
                        // 可以選擇手動更新欄位，避免 RowVersion 衝突
                    }

                    //需要更新直銷部分
                    var referrerMember = await _dbContext.ReferrerMembers.FindAsync(userId);
                    if (referrerMember == null)
                    {
                        referrerMember = new ReferrerMember
                        {
                            Id = userId,
                            Name = UserData.Name
                        };
                        _dbContext.ReferrerMembers.Add(referrerMember);
                    }
                    else
                    {
                        referrerMember.Name = UserData.Name;
                    }

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