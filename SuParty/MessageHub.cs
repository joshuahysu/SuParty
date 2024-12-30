using MessagePack;
using Microsoft.AspNetCore.SignalR;
using SuParty.Pages.Chat;
using System.Security.Claims;

namespace SuParty
{
    public class MessageHub : Hub
    {
        //private readonly ApplicationDbContext _context;

        //public MessageHub(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

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
        //SendMessage
        // 廣播訊息給指定聊天室群組
        public async Task SM(MessageModel message)
        {
            //string username = Context.User.Identity.Name;
            // 儲存到檔案系統
            ChatStorage.SaveMessage(message.ChatroomId, message);

            // 發送訊息到指定的聊天室群組ReceiveMessage=RM
            await Clients.Group(message.ChatroomId).SendAsync("RM", message.Name, message.Content);
        }
        /// <summary>
        /// SendMessagePrivate
        /// 廣播訊息給指定聊天室群組(不用填id)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>

        public async Task S(MessageModel message)
        {
            string username = Context.User.Identity.Name;

            //暫時先存
            message.Name=username;
            // 取得目前使用者的 ID
            message.UserId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 儲存到檔案系統
            ChatStorage.SaveMessage(message.ChatroomId, message);

            // 發送訊息到指定的聊天室群組ReceiveMessage=RM
            await Clients.Group(message.ChatroomId).SendAsync("RM", username, message.Content);
        }        
    }
    [MessagePackObject]
    public class MessageModel
    {
        [Key(0)]
        public string Name { get; set; } = "";
        [Key(1)]
        public string Content { get; set; } = "";
        [Key(2)]
        public DateTime CreatedAt { get; set; }
        [Key(3)]
        public string ChatroomId { get; set; } = "";
        [Key(4)]
        public string UserId { get; set; } = "";
    }
}