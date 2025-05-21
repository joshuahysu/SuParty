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
        public RealEstateUserData UserData { get; set; } // �j�w UserData �ҫ�

        private readonly ApplicationDbContext _dbContext;

        public EditUserDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> OnGet()
        {
            UserData = _dbContext.UserDatas.Find(User.FindFirstValue(ClaimTypes.NameIdentifier));
            // �T�O�b�����[�������� User ���O null
            if (UserData == null)
            {
                UserData = new RealEstateUserData(); // ��l�� User ����
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
                    UserData.Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    //�۰�insert or update
                    _dbContext.UserDatas.Update(UserData);

                    //�ݭn��s���P����
                    ReferrerMember referrerMember = _dbContext.ReferrerMembers.Find(UserData.Id);

                    if (referrerMember == null)
                    {
                        referrerMember = new ReferrerMember() {
                            Id=UserData.Id,
                        }; // ��l�� referrerMember ����
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