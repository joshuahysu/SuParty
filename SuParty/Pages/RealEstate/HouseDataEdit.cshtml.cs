﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id != null)
            {
                HouseData = await _dbContext.HouseDatas.FindAsync(id);
            }
            else {
                HouseData=new HouseData();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // 如果表單不合法，保持在頁面上顯示錯誤
            }

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                HouseData.SalesId = userId;
                if (!_dbContext.HouseDatas.Any(h => h.Id == HouseData.Id))
                {
                    // 没有 ID，插入新数据
                    HouseData.Id = Guid.NewGuid().ToString(); // 生成新的 GUID                   

                    _dbContext.HouseDatas.Add(HouseData); // 插入
                }
                else
                {
                    _dbContext.HouseDatas.Update(HouseData);
                }

                _dbContext.SaveChanges();

            }
            else {
                return RedirectToPage("/Account/Login");
            }

            // 重定向到房屋詳情頁
            return RedirectToPage("/RealEstate/HouseData", new { id = HouseData.Id });

        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _dbContext.HouseDatas.Remove(new HouseData { Id = id });
                await _dbContext.SaveChangesAsync();
                return new JsonResult(new { success = true, message = "Delete success" });
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
        }
    }
}