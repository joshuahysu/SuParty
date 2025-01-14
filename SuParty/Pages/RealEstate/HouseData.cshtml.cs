using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using SuParty.Data.DataModel.RealEstate;
using System.Security.Claims;

namespace SuParty.Pages.RealEstate
{
    public class HouseDataModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public HouseData? HouseData { get; set; } = new HouseData();

        public HouseDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult OnGet(string id)
        {
            HouseData = _dbContext.HouseDatas.Find(id);
            if (User.Identity.IsAuthenticated)
            {
                //���|���]�\�ק����

            }
            else
            {
  
            }
            
            return Page();
        }
        public IActionResult OnPostTraceRealEstates(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // �d�ߨϥΪ�
                UserData? user = _dbContext.UserDatas.Find(userId);

                // �s�W�@����ƨ� �l�ܦC��
                user.TraceRealEstates.Add(id);

                // �O�s�ܧ���Ʈw
                _dbContext.SaveChanges();                

            }
            return new JsonResult(new { success = true, message = "Add success" });
        }        
    }
}