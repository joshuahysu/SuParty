using SuParty.Data;
using System;
using System.IO;
using System.Text.Json;

namespace SuParty.Pages.Chat
{

    public class ChatStorage
    {
        private const string BasePath = "messages";
        /// <summary>
        /// 儲存訊息
        /// </summary>
        /// <param name="chatroomId"></param>
        /// <param name="message"></param>
        public static void SaveMessage(string chatroomId, Message message)
        {
            // 確保基礎目錄存在
            Directory.CreateDirectory(BasePath);

            // 聊天室目錄
            string chatroomPath = Path.Combine(BasePath, chatroomId);
            Directory.CreateDirectory(chatroomPath);

            // 檔案名稱以日期命名
            string fileName = $"{message.CreatedAt:yyyy-MM-dd}.txt";
            string filePath = Path.Combine(chatroomPath, fileName);

            // 將訊息序列化為 JSON 格式
            string serializedMessage = JsonSerializer.Serialize(message);

            // 寫入檔案
            File.AppendAllText(filePath, serializedMessage + Environment.NewLine);
        }


        /// <summary>
        /// 讀取指定聊天室和日期的訊息
        /// </summary>
        /// <param name="chatroomId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<Message> ReadMessages(string chatroomId, DateTime? date=null)
        {
            if(date==null)
                date=DateTime.Now;
            string chatroomPath = Path.Combine("messages", chatroomId);
            string fileName = $"{date:yyyy-MM-dd}.txt";
            string filePath = Path.Combine(chatroomPath, fileName);

            var messages = new List<Message>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    // 將每行 JSON 反序列化為 Message 物件
                    var message = JsonSerializer.Deserialize<Message>(line);
                    if (message != null)
                    {
                        messages.Add(message);
                    }
                }
            }

            return messages;
        }
    }
}