using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
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
        public IActionResult OnGet(string userId)
        {

            if (User.Identity.IsAuthenticated)
            {
                // ���o�n�J�̪��b���]�Τ�W�ιq�l�l��^
                string username = User.Identity.Name;
                if(String.IsNullOrEmpty(userId))
                     userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // �ϥεn�J�̱b������L�B�z
                //Message =$"Logged in as: {username}";
                //�ثe�����}
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

        public IActionResult OnPostTracking(string id,bool tracking)
        {
            if (User.Identity.IsAuthenticated)
            {
                // ���o�n�J�̪��b���]�Τ�W�ιq�l�l��^
                string username = User.Identity.Name;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var dbtracking = _dbContext.Trackings.Find(userId);
                if (tracking)
                {
                    //���s�b�~�s�W
                    if (!dbtracking.TrackingList.Any(t => t == id))
                    {
                        dbtracking.TrackingList.Add(id);
                    }
                }
                else
                    dbtracking.TrackingList.Remove(id);
                _dbContext.SaveChanges();
                return Page();
            }
            else
            {
                Message = "�z�|���n�J�A�N���w�V��n�J�����C";
                //return RedirectToPage("/Account/Login");
            }

            return Page();
        }
        
    }
}
