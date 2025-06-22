using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Pages.Chat;
using System.Security.Claims;

namespace SuParty.Pages
{
    public class ChatPrivateModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public string UserId = "";

        public ChatPrivateModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // 模擬留言資料來源（可以換成資料庫或其他儲存方式）
        public List<MessageModel> Messages { get; private set; }

        public List<String> Chatrooms { get; private set; } = new List<String>();
        
        public async Task<IActionResult> OnGet(string? chatroomId = null)
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Identity/Account/Login");
            // 取得登入者的帳號（用戶名或電子郵件）
            string username = User.Identity.Name;
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // 使用登入者帳號做其他處理

            var UserData = await _dbContext.UserDatas.FindAsync(UserId);

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
                    Messages = new List<MessageModel>();
                    _dbContext.SaveChanges();
                }
                else
                {
                    // 讀取指定聊天室的訊息
                    Messages = ChatStorage.ReadAndMergeFilesDefalut(chatroomId);
                }
            }
            //擁有的聊天室
            if(UserData!=null)
            Chatrooms = UserData.ChatRooms.ToList();
            return Page();

        }
        
        public async Task<IActionResult> OnPostDeleteChatRoom(string chatroomId) 
        {
            if (!User.Identity.IsAuthenticated)     
                return RedirectToPage("/Account/Login");
            
            // 取得登入者的帳號（用戶名或電子郵件）
            string username = User.Identity.Name;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // 使用登入者帳號做其他處理

            var UserData = await _dbContext.UserDatas.FindAsync(userId);

            if (string.IsNullOrEmpty(chatroomId))
            {
                return Page();
            }
            else
            {
                //刪除
                if (UserData.ChatRooms.Remove(chatroomId))
                {
                    await _dbContext.SaveChangesAsync();
                    ChatStorage.DeleteChatroom(chatroomId);
                }
            }
            //擁有的聊天室
            Chatrooms = UserData.ChatRooms.ToList();
            return new JsonResult(new { success = false, message = "delete success" });            
        }
    }
}