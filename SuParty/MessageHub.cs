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
        //SendMessage
        // �s���T�������w��ѫǸs��
        public async Task SM(MessageModel message)
        {
            //string username = Context.User.Identity.Name;
            // �x�s���ɮרt��
            ChatStorage.SaveMessage(message.ChatroomId, message);

            // �o�e�T������w����ѫǸs��ReceiveMessage=RM
            await Clients.Group(message.ChatroomId).SendAsync("RM", message.Name, message.Content);
        }
        /// <summary>
        /// SendMessagePrivate
        /// �s���T�������w��ѫǸs��(���ζ�id)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>

        public async Task S(MessageModel message)
        {
            string username = Context.User.Identity.Name;

            //�Ȯɥ��s
            message.Name=username;
            // ���o�ثe�ϥΪ̪� ID
            message.UserId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // �x�s���ɮרt��
            ChatStorage.SaveMessage(message.ChatroomId, message);

            // �o�e�T������w����ѫǸs��ReceiveMessage=RM
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