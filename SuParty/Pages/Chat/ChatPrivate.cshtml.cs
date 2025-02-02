using Microsoft.AspNetCore.Mvc;
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
        // �����d����ƨӷ��]�i�H������Ʈw�Ψ�L�x�s�覡�^
        public List<MessageModel> Messages { get; private set; }

        public List<String> Chatrooms { get; private set; } = new List<String>();
        public IActionResult OnGet(string? chatroomId = null)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Account/Login");
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
                //�x�s�oUser����ƥN��L���b�o��ѫǸ�
                if (!UserData.ChatRooms.Contains(chatroomId))
                {
                    UserData.ChatRooms.Add(chatroomId);
                    Messages = new List<MessageModel>();
                    _dbContext.SaveChanges();
                }
                else
                {
                    // �ھ� chatroomId ��l�Ưd���C��
                    Messages = ChatStorage.ReadAndMergeFilesDefalut(chatroomId);
                }
            }
            //�֦�����ѫ�
            Chatrooms = UserData.ChatRooms.ToList();
            return Page();

        }

        public IActionResult OnPostDeleteChatRoom(string chatroomId) 
        {
            if (!User.Identity.IsAuthenticated)     
                return RedirectToPage("/Account/Login");
            
            // ���o�n�J�̪��b���]�Τ�W�ιq�l�l��^
            string username = User.Identity.Name;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // �ϥεn�J�̱b������L�B�z

            var UserData = _dbContext.UserDatas.Find(userId);

            if (string.IsNullOrEmpty(chatroomId))
            {
                return Page();
            }
            else
            {
                //�R��
                if (UserData.ChatRooms.Remove(chatroomId))
                {
                    _dbContext.SaveChanges();
                    ChatStorage.DeleteChatroom(chatroomId);
                }
            }
            //�֦�����ѫ�
            Chatrooms = UserData.ChatRooms.ToList();
            return new JsonResult(new { success = false, message = "delete success" });            
        }
    }
}