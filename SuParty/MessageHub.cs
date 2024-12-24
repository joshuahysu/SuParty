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

        // ��Τ�s���ɡA�[�J��ѫǸs��
        public override async Task OnConnectedAsync()
        {
            // ���] chatroomId �O�q�L QueryString �ζǤJ��
            var chatroomId = Context.GetHttpContext().Request.Query["chatroomId"];

            if (!string.IsNullOrEmpty(chatroomId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatroomId);
            }

            await base.OnConnectedAsync();
        }

        // ��Τ��_�}�s���ɡA���}��ѫǸs��
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var chatroomId = Context.GetHttpContext().Request.Query["chatroomId"];

            if (!string.IsNullOrEmpty(chatroomId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatroomId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        // �s���T�������w��ѫǸs��
        public async Task SendMessage(MessageModel message)
        {
            // ������
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
            // �x�s���ɮרt��
            ChatStorage.SaveMessage(message.ChatroomId, newMessage);

            // �o�e�T������w����ѫǸs��
            await Clients.Group(chatroomId).SendAsync("ReceiveMessage", name, content);
        }
    }
}