using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
namespace SuParty.Pages.User
{
    [AllowAnonymous]
    public class UploadIMGModel : PageModel
    {
        private string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads","img");

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public string FilePath { get; set; }

        public void OnGet()
        {
            // 初始化頁面時不需要執行額外動作
        }
        //[Authorize]
        public async Task<IActionResult> OnPostAsync()
        {
            string userId = "All";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // 取得登入者的帳號（用戶名或電子郵件）
                //username = User.Identity.Name;
            }
            try
            {
                if (UploadedFile == null || UploadedFile.Length == 0)
                {
                    ModelState.AddModelError(string.Empty, "請選擇一張圖片上傳。");
                    return Page();
                }
                _uploadPath = Path.Combine(_uploadPath, userId);
                // 確保上傳目錄存在
                if (!Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }

                // 設定檔案路徑
                string fileName = Path.GetRandomFileName() + Path.GetExtension(UploadedFile.FileName);
                var filePath = Path.Combine(_uploadPath, fileName);

                // 將圖片儲存到伺服器
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedFile.CopyToAsync(stream);
                }
                // 回傳前端可以訪問的 URL，這是相對於 wwwroot 的路徑
                FilePath = "/uploads/img/" + userId + "/" + fileName;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"上傳過程中發生錯誤：{ex.Message}");
            }
            return Page();
        }
    }
}