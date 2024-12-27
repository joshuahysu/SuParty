using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuParty.Data;
using SuParty.Pages.Chat;
using System.Security.Claims;
using static SuParty.MessageHub;

namespace SuParty.Pages
{
    public class ChatPrivateModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatPrivateModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // 模擬留言資料來源（可以換成資料庫或其他儲存方式）
        public List<MessageModel> Messages { get; private set; }

        public List<String> Chatrooms { get; private set; }=new List<String>();
        public void OnGet(string? chatroomId = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                // 取得登入者的帳號（用戶名或電子郵件）
                string username = User.Identity.Name;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // 使用登入者帳號做其他處理

                var UserData = _dbContext.UserDatas.Find(userId);

                if (string.IsNullOrEmpty(chatroomId))
                {
                    // 如果未提供 chatroomId，處理預設邏輯
                    Messages = new List<MessageModel>();
                }
                else
                {
                    //儲存這User的資料代表他有在這聊天室裡
                    if (!UserData.ChatRooms.Contains(chatroomId))
                    {
                        UserData.ChatRooms.Add(chatroomId);
                        _dbContext.SaveChanges();
                    }             
                    // 根據 chatroomId 初始化留言列表
                    Messages = ChatStorage.ReadMessages(chatroomId);
                }

                Chatrooms = UserData.ChatRooms.ToList();
            }
            else
            {
               // Message = "您尚未登入，將重定向到登入頁面。";
                //return RedirectToPage("/Account/Login");
            }

        }
    }
}