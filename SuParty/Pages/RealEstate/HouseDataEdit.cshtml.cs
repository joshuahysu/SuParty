using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using SuParty.Data.DataModel.RealEstate;
using System.Security.Claims;

namespace SuParty.Pages.RealEstate
{
    public class HouseDataEditModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public HouseDataEditModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public HouseData HouseData { get; set; }

        public void OnGet(int? id)
        {
            if (id != null)
            {
                HouseData = _dbContext.HouseDatas.Find(id);
            }
            else {
                HouseData=new HouseData();
                    }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // �p�G��椣�X�k�A�O���b�����W��ܿ��~
            }

            _dbContext.HouseDatas.Update(HouseData);
            _dbContext.SaveChanges();
            // ���w�V��ЫθԱ���
            return RedirectToPage("../HouseData", new { id = HouseData.Id });
        }
    }
}