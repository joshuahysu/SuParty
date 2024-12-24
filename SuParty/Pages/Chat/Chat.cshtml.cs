using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Pages.Chat;

namespace SuParty.Pages
{
    public class ChatModel : PageModel
    {
        // 模擬留言資料來源（可以換成資料庫或其他儲存方式）
        public List<Message> Messages { get; private set; }
        public void OnGet(string? chatroomId = null)
        {
            if (string.IsNullOrEmpty(chatroomId))
            {
                // 如果未提供 chatroomId，處理預設邏輯
                Messages = new List<Message>();
            }
            else
            {
                // 根據 chatroomId 初始化留言列表
                Messages = ChatStorage.ReadMessages(chatroomId);
            }
        }
    }
}