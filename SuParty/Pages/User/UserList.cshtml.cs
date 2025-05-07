using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Data.DataModel;
using System.Security.Claims;
namespace SuParty.Pages.User
{
    [AllowAnonymous]
    public class UserListModel : PageModel
    {
        public List<RealEstateUserData> UserDataList { get; set; } = new ();

        private readonly ApplicationDbContext _dbContext;

        public UserListModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet(int page = 1)
        {
            // ���X�p�G���ǻ��A�h�|�ϥιw�]�� 1
            int pageSize = 10;  // �C����ܪ���Ƽƶq

            // �d�ߤ������
            UserDataList = _dbContext.UserDatas
                .Skip((page - 1) * pageSize) // ���L�e���w��ܪ����
                .Take(pageSize)                    // ���o���w���ƪ����
                .ToList();

            // ���o����`��
            int totalCount = _dbContext.UserDatas.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // �ǻ���ƨ�e��
            ViewData["PagedData"] = UserDataList;
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = page;
        }


        //public void OnGet()
        //{ 

            //string folderPath = @"C:\Your\Folder\Path"; // �������A�n����Ƨ����|

            //// ���o��Ƨ��U�Ҧ��ɮת�������|
            //string[] filePaths = Directory.GetFiles(folderPath);

            //// ���o�ɮצW
            //foreach (var filePath in filePaths)
            //{
            //    string fileName = Path.GetFileName(filePath); // ���o�ɮצW�١]���]�t���|�^
            //    Console.WriteLine(fileName); // ��X�ɮצW��
            //}

        //}
    }
}