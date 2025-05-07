using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using System.Security.Claims;

namespace SuParty.Pages.RealEstate
{
    public class ProductDataEditModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductDataEditModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public ProductData? ProductData { get; set; }

        [BindProperty]
        public List<IFormFile> ImagesUpload { get; set; } = new();
        public async Task<IActionResult> OnGet(int? id)
        {
            if (id != null)
            {
                ProductData = await _dbContext.ProductDatas.FindAsync(id);
            }
            else {
                ProductData = new ProductData();
            }
            return Page();
        }

        /// <summary>
        /// 新增一筆
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // 如果表單不合法，保持在頁面上顯示錯誤
            }

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ProductData.SalesId = userId;


                //上傳圖片
                var uploadsFolder = Path.Combine("wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder);

                foreach (var formFile in ImagesUpload)
                {
                    if (formFile.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        ProductData.Images.Add("/uploads/" + uniqueFileName); // 儲存相對路徑
                    }
                }

                if (!_dbContext.ProductDatas.Any(h => h.Id == ProductData.Id))
                {
                    // 没有 ID，插入新数据
                    ProductData.Id = Guid.NewGuid().ToString(); // 生成新的 GUID                   

                    _dbContext.ProductDatas.Add(ProductData); // 插入
                }
                else
                {
                    _dbContext.ProductDatas.Update(ProductData);
                }

                _dbContext.SaveChanges();

            }
            else {
                return RedirectToPage("/Account/Login");
            }

            // 重定向到商品詳情頁
            return RedirectToPage("/Product/ProductData", new { id = ProductData.Id });

        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _dbContext.ProductDatas.Remove(new ProductData { Id = id });
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