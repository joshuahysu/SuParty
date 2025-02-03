using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using SuParty.Data.DataModel.RealEstate;
using SuParty.Service.Qrcode;
using System.Security.Claims;

namespace SuParty.Pages.RealEstate
{
    public class HouseDataModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public HouseData? HouseData { get; set; } = new HouseData();

        public string Base64QRCode { get; set; } = "";


        public HouseDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            HouseData = await _dbContext.HouseDatas.FindAsync(id);
            var user=await _dbContext.UserDatas.FindAsync(HouseData.SalesId);
            Base64QRCode = Qrcode.CreateQrcode(user.Line_Url);
            if (User.Identity.IsAuthenticated)
            {
                //���|���]�\�ק����

            }
            else
            {
  
            }
            
            return Page();
        }
        public async Task<IActionResult> OnPostTraceRealEstates(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // �d�ߨϥΪ�
                UserData? user = await _dbContext.UserDatas.FindAsync(userId);

                // �s�W�@����ƨ� �l�ܦC��
                user.TraceRealEstates.Add(id);

                // �O�s�ܧ���Ʈw
                await _dbContext.SaveChangesAsync();                

            }
            return new JsonResult(new { success = true, message = "Add success" });
        }        
    }
}