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
            // 頁碼如果未傳遞，則會使用預設值 1
            int pageSize = 10;  // 每頁顯示的資料數量

            // 查詢分頁資料
            UserDataList = _dbContext.UserDatas
                .Skip((page - 1) * pageSize) // 跳過前面已顯示的資料
                .Take(pageSize)                    // 取得指定頁數的資料
                .ToList();

            // 取得資料總數
            int totalCount = _dbContext.UserDatas.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // 傳遞資料到前端
            ViewData["PagedData"] = UserDataList;
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = page;
        }


        //public void OnGet()
        //{ 

            //string folderPath = @"C:\Your\Folder\Path"; // 替換為你要的資料夾路徑

            //// 取得資料夾下所有檔案的完整路徑
            //string[] filePaths = Directory.GetFiles(folderPath);

            //// 取得檔案名
            //foreach (var filePath in filePaths)
            //{
            //    string fileName = Path.GetFileName(filePath); // 取得檔案名稱（不包含路徑）
            //    Console.WriteLine(fileName); // 輸出檔案名稱
            //}

        //}
    }
}