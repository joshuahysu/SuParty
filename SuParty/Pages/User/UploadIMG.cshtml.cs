using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using System.Reflection;

namespace SuParty.Pages.User
{
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

        public async Task<IActionResult> OnPostAsync()
        {
            string username = "All";
            if (User.Identity.IsAuthenticated)
            {
                // 取得登入者的帳號（用戶名或電子郵件）
                username = User.Identity.Name;

            }
            try
            {
                if (UploadedFile == null || UploadedFile.Length == 0)
                {
                ModelState.AddModelError(string.Empty, "請選擇一張圖片上傳。");
                return Page();
                }
                _uploadPath = Path.Combine(_uploadPath, username);
                // 確保上傳目錄存在
                if (!Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }

                // 設定檔案路徑
                string fileName = Path.GetRandomFileName() + Path.GetExtension(UploadedFile.FileName);
                FilePath = Path.Combine(_uploadPath, fileName);

                // 將圖片儲存到伺服器
                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    await UploadedFile.CopyToAsync(stream);
                }
                 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"上傳過程中發生錯誤：{ex.Message}");
                return Page();
            }
            return Page();
        }
    }
}