

namespace SuParty.Pages.Chat
{
    public class ChatService
    {
        public static void Start(string chatroomId, MessageModel message)
        {
            ChatStorage.SaveMessage(chatroomId,message);
        }
    }
}