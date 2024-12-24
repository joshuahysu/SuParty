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
            // ��l�ƭ����ɤ��ݭn�����B�~�ʧ@
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string username = "All";
            if (User.Identity.IsAuthenticated)
            {
                // ���o�n�J�̪��b���]�Τ�W�ιq�l�l��^
                username = User.Identity.Name;

            }
            try
            {
                if (UploadedFile == null || UploadedFile.Length == 0)
                {
                ModelState.AddModelError(string.Empty, "�п�ܤ@�i�Ϥ��W�ǡC");
                return Page();
                }
                _uploadPath = Path.Combine(_uploadPath, username);
                // �T�O�W�ǥؿ��s�b
                if (!Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }

                // �]�w�ɮ׸��|
                string fileName = Path.GetRandomFileName() + Path.GetExtension(UploadedFile.FileName);
                FilePath = Path.Combine(_uploadPath, fileName);

                // �N�Ϥ��x�s����A��
                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    await UploadedFile.CopyToAsync(stream);
                }
                 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"�W�ǹL�{���o�Ϳ��~�G{ex.Message}");
                return Page();
            }
            return Page();
        }
    }
}