using Microsoft.AspNetCore.SignalR;
using SuParty.Data;
using SuParty.Pages;
using SuParty.Pages.Chat;

namespace SuParty
{
    public class MessageHub : Hub
    {
        private readonly ApplicationDbContext _context;
        public class MessageModel
        {
            public string Name { get; set; }
            public string Content { get; set; }
            public string Timestamp { get; set; }
            public string ChatroomId { get; set; }
            public string UserId { get; set; }
        }

        public MessageHub(ApplicationDbContext context)
        {
            _context = context;
        }

        // 當用戶連接時，加入聊天室群組
        public override async Task OnConnectedAsync()
        {
            // 假設 chatroomId 是通過 QueryString 或傳入的
            var chatroomId = Context.GetHttpContext().Request.Query["chatroomId"];

            if (!string.IsNullOrEmpty(chatroomId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatroomId);
            }

            await base.OnConnectedAsync();
        }

        // 當用戶斷開連接時，離開聊天室群組
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var chatroomId = Context.GetHttpContext().Request.Query["chatroomId"];

            if (!string.IsNullOrEmpty(chatroomId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatroomId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        // 廣播訊息給指定聊天室群組
        public async Task SendMessage(MessageModel message)
        {
            // 獲取資料
            string name = message.Name;
            string content = message.Content;
            string chatroomId = message.ChatroomId;
            string timestamp = message.Timestamp;
            string userId = message.UserId;
            
            var newMessage = new Message
            {
                Name = name,
                Content = content,
                CreatedAt = DateTime.Now
            };
            // 儲存到檔案系統
            ChatStorage.SaveMessage(message.ChatroomId, newMessage);

            // 發送訊息到指定的聊天室群組
            await Clients.Group(chatroomId).SendAsync("ReceiveMessage", name, content);
        }
    }
}