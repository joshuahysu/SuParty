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
        public RealEstateUserData UserData { get; set; } // �j�w UserData �ҫ�

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
                // �T�O�b�����[�������� User ���O null
                if (UserData == null)
                {
                UserData = new RealEstateUserData(); // ��l�� User ����
                }
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }
            return Page();
        }

        /// <summary>
        /// ��s�ӤH���
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

                    // ���d�߸�Ʈw���O�_�w�s�b�ӨϥΪ̸��
                    var existingUserData = _dbContext.UserDatas.AsNoTracking().FirstOrDefault(h => h.Id == userId);

                    if (existingUserData == null)
                    {
                        _dbContext.UserDatas.Add(UserData); // �s�W
                    }
                    else
                    {
                        _dbContext.UserDatas.Update(UserData);
                        // �i�H��ܤ�ʧ�s���A�קK RowVersion �Ĭ�
                    }

                    //�ݭn��s���P����
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