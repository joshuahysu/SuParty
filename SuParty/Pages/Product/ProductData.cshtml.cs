using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using System.Security.Claims;
using System.Text.Json;

namespace SuParty.Pages.Product
{
    public class ProductDataModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductData? ProductData { get; set; } = new ProductData();

        public ProductDataModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult OnGet(string id)
        {
            ProductData = _dbContext.ProductDatas.Find(id);
            if (User.Identity.IsAuthenticated)
            {
                //���|���]�\�ק����

            }
            else
            {
  
            }
            
            return Page();
        }
        public IActionResult OnPostAddShoppingCart(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // �d�ߨϥΪ�
                var user = _dbContext.UserDatas.Find(userId);

                // �s�W�@����ƨ� ShoppingCart
                user.ShoppingCart.Add(new ProductData { Id = id });

                // �O�s�ܧ���Ʈw
                _dbContext.SaveChanges();                

            }
            return new JsonResult(new { success = true, message = "Add success" });
        }


        public async Task<IActionResult> OnPostTracking(string id, bool tracking, int loveScore)
        {
            if (User.Identity.IsAuthenticated)
            {
                // ���o�n�J�̪��b���]�Τ�W�ιq�l�l��^
                string username = User.Identity.Name;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var dbtracking = await _dbContext.Trackings.FindAsync(userId);
                if (dbtracking == null)
                {
                    dbtracking = new Tracking();
                    dbtracking.Id = userId;
                    dbtracking.TrackingList.Add(new TrackingObject()
                    {
                        LoveScore = loveScore,
                        Id = id
                    });
                    _dbContext.Trackings.Add(dbtracking);
                }
                if (tracking)
                {
                    //���s�b�~�s�W
                    if (!dbtracking.TrackingList.Any(t => t.Id == id))
                    {
                        dbtracking.TrackingList.Add(new TrackingObject()
                        {
                            LoveScore = loveScore,
                            Id = id
                        });
                    }
                }
                else
                    {
                        dbtracking.TrackingList.Remove(new TrackingObject()
                        {
                            LoveScore = loveScore,
                            Id = id
                        });
                }
                await _dbContext.SaveChangesAsync();
                var successResponse = new { success = true, message = "�l�ܪ��A��s���\�C" };
                return Content(JsonSerializer.Serialize(successResponse), "application/json");
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }
        }
    }
}