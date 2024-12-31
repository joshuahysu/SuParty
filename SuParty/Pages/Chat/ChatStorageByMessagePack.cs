using MessagePack;
using SuParty.Data;
using SuParty.Service.UseGoogleProtobuf;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace SuParty.Pages.Chat
{
    /// <summary>
    /// 失敗品
    /// </summary>
    public class ChatStorageByMessagePack
    {
        private const string BasePath = "messages";
        /// <summary>
        /// 儲存訊息
        /// </summary>
        /// <param name="chatroomId"></param>
        /// <param name="message"></param>
        public static void SaveMessage(string chatroomId, MessageModel message)
        {
            // 確保基礎目錄存在
            Directory.CreateDirectory(BasePath);

            // 聊天室目錄
            string chatroomPath = Path.Combine(BasePath, chatroomId);
            Directory.CreateDirectory(chatroomPath);

            // 檔案名稱以日期命名
            string fileName = $"{message.CreatedAt:yyyy-MM-dd}.dat";
            string filePath = Path.Combine(chatroomPath, fileName);

            // 逐條訊息寫入檔案
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(stream))
            {
                // 使用 MessagePack 來編碼
                byte[] jsonBytes = MessagePackSerializer.Serialize(message);

                // 寫入訊息的位元組
                writer.Write(jsonBytes);

                // 寫入換行符號 (0x0A)
                writer.Write((byte)0x0A);

            }
        }

        /// <summary>
        /// 讀取指定聊天室的訊息
        /// </summary>
        /// <param name="chatroomId"></param>
        /// <returns></returns>
        public static List<MessageModel> ReadAndMergeFilesDefalut(string chatroomId)
        {
            string folderPath = Path.Combine("messages", chatroomId);// 資料夾路徑

            // 檢查資料夾是否存在，如果不存在，則建立資料夾
            if (!Directory.Exists(folderPath))
            {   
                Directory.CreateDirectory(folderPath); // 創建資料夾   
            }

            // 列出資料夾內所有符合 yyyy-MM-dd.txt 格式的檔案
            var validFiles = Directory.GetFiles(folderPath, "*.dat")
                                      .Select(Path.GetFileNameWithoutExtension)
                                      .Where(fileName => DateTime.TryParseExact(fileName, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                                      .OrderBy(fileName => fileName) // 按日期降序排列
                                      .ToList();

            if (!validFiles.Any())
            {
                Console.WriteLine("資料夾內沒有符合格式的檔案。");
                return new List<MessageModel>();
            }

            // 開始處理檔案
            var messages = ReadAndMergeFiles(folderPath, validFiles);
            return messages;
        }

        public static List<MessageModel> ReadAndMergeFiles(string folderPath, List<string> validFiles = null)
        {
            List<MessageModel> messages = new List<MessageModel>();

            foreach (var fileName in validFiles)
            {
                string filePath = Path.Combine(folderPath, $"{fileName}.dat");

                if (File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (var reader = new BinaryReader(stream))
                    {
                        while (reader.BaseStream.Position < reader.BaseStream.Length)
                        {
                            // 讀取一條訊息
                            byte[] messageBytes = ReadMessage(reader);

                            if (messageBytes != null)
                            {
                                // 反序列化訊息
                                var message = MessagePackSerializer.Deserialize<MessageModel>(messageBytes);
                                messages.Add(message);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"找不到檔案: {fileName}.txt");
                }

                // 如果資料筆數已達到或超過 100，結束處理
                if (messages.Count >= 100)
                {
                    break;
                }
            }

            return messages;
        }

        // 讀取單一訊息直到換行符
        private static byte[] ReadMessage(BinaryReader reader)
        {
            using (var memoryStream = new MemoryStream())
            {
                byte currentByte;
                // 讀取直到換行符 (0x0A)
                while ((currentByte = reader.ReadByte()) != 0x0A)
                {
                    memoryStream.WriteByte(currentByte);
                }
                return memoryStream.ToArray();
            }
        }

    }
}