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
            // ��l�ƭ����ɤ��ݭn�����B�~�ʧ@
        }
        //[Authorize]
        public async Task<IActionResult> OnPostAsync()
        {
            string userId = "All";
            if (User.Identity.IsAuthenticated)
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // ���o�n�J�̪��b���]�Τ�W�ιq�l�l��^
                //username = User.Identity.Name;
            }
            try
            {
                if (UploadedFile == null || UploadedFile.Length == 0)
                {
                    ModelState.AddModelError(string.Empty, "�п�ܤ@�i�Ϥ��W�ǡC");
                    return Page();
                }
                _uploadPath = Path.Combine(_uploadPath, userId);
                // �T�O�W�ǥؿ��s�b
                if (!Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }

                // �]�w�ɮ׸��|
                string fileName = Path.GetRandomFileName() + Path.GetExtension(UploadedFile.FileName);
                var filePath = Path.Combine(_uploadPath, fileName);

                // �N�Ϥ��x�s����A��
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedFile.CopyToAsync(stream);
                }
                // �^�ǫe�ݥi�H�X�ݪ� URL�A�o�O�۹�� wwwroot �����|
                FilePath = "/uploads/img/" + userId + "/" + fileName;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"�W�ǹL�{���o�Ϳ��~�G{ex.Message}");
            }
            return Page();
        }
    }
}