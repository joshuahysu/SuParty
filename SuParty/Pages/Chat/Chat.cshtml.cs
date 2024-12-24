using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Data;
using SuParty.Pages.Chat;

namespace SuParty.Pages
{
    public class ChatModel : PageModel
    {
        // �����d����ƨӷ��]�i�H������Ʈw�Ψ�L�x�s�覡�^
        public List<Message> Messages { get; private set; }
        public void OnGet(string? chatroomId = null)
        {
            if (string.IsNullOrEmpty(chatroomId))
            {
                // �p�G������ chatroomId�A�B�z�w�]�޿�
                Messages = new List<Message>();
            }
            else
            {
                // �ھ� chatroomId ��l�Ưd���C��
                Messages = ChatStorage.ReadMessages(chatroomId);
            }
        }
    }
}