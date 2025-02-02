using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuParty.Data;
using SuParty.Pages.Chat;
using System.Security.Claims;
using static SuParty.MessageHub;

namespace SuParty.Pages
{
    public class ChatModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // �����d����ƨӷ��]�i�H������Ʈw�Ψ�L�x�s�覡�^
        public List<MessageModel> Messages { get; private set; }

        public List<String> Chatrooms { get; private set; }=new List<String>();
        public IActionResult OnGet(string? chatroomId = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                // ���o�n�J�̪��b���]�Τ�W�ιq�l�l��^
                string username = User.Identity.Name;
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // �ϥεn�J�̱b������L�B�z

                var UserData = _dbContext.UserDatas.Find(userId);

                if (string.IsNullOrEmpty(chatroomId))
                {
                    // �p�G������ chatroomId�A�B�z�w�]�޿�
                    Messages = new List<MessageModel>();
                }
                else
                {
                    if (!UserData.ChatRooms.Contains(chatroomId))
                    {
                        UserData.ChatRooms.Add(chatroomId);
                        _dbContext.SaveChanges();
                    }             
                    // �ھ� chatroomId ��l�Ưd���C��
                    Messages = ChatStorage.ReadMessages(chatroomId);
                }

                Chatrooms = UserData.ChatRooms.ToList();
                return Page();
            }
            else
            {
               // Message = "�z�|���n�J�A�N���w�V��n�J�����C";
                return RedirectToPage("/Account/Login");
            }

        }
    }
}