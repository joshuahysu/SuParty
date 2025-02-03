using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using System.Security.Claims;
using System.Text.Json;

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
        public async Task<IActionResult> OnGet(string userId)
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
                UserData = await _dbContext.UserDatas.FindAsync(userId);

                return Page();
            }
            else
            {
                Message = "�z�|���n�J�A�N���w�V��n�J�����C";
                return RedirectToPage("/Account/Login");
            }

        }

        public async Task<IActionResult> OnPostTracking(string id,bool tracking)
        {
            if (User.Identity.IsAuthenticated)
            {
                // ���o�n�J�̪��b���]�Τ�W�ιq�l�l��^
                string username = User.Identity.Name;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var dbtracking = await _dbContext.Trackings.FindAsync(userId);
                if (dbtracking == null)
                {
                    dbtracking=new Tracking();
                    dbtracking.Id = userId;
                    _dbContext.Trackings.Add(dbtracking);
                }
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
                await _dbContext.SaveChangesAsync();
                var successResponse = new { success = true, message = "�l�ܪ��A��s���\�C" };
                return Content(JsonSerializer.Serialize(successResponse), "application/json");
            }
            else
            {
                Message = "�z�|���n�J�A�N���w�V��n�J�����C";
                return RedirectToPage("/Account/Login");
            }
        }        
    }
}