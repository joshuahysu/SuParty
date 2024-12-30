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

            // 使用 MessagePack 來編碼
            byte[] data = MessagePackSerializer.Serialize(message);
            // 將資料寫入檔案
            //File.AppendAllBytes(filePath, data);
            // 打開檔案並將資料追加
            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                fs.Write(data, 0, data.Length);
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
                                      .OrderByDescending(fileName => fileName) // 按日期降序排列
                                      .ToList();

            if (!validFiles.Any())
            {
                Console.WriteLine("資料夾內沒有符合格式的檔案。");
                return new List<MessageModel>();
            }

            //Console.WriteLine("資料夾內的檔案清單：");
            //validFiles.ForEach(file => Console.WriteLine(file));

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
                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (var reader = new BinaryReader(fs))
                    {
                        // 假設每行是一個序列化的 MessagePack 資料
                        while (reader.BaseStream.Position < reader.BaseStream.Length)
                        {
                            // 讀取資料長度
                            int length = reader.ReadInt32();

                            // 讀取資料
                            byte[] data = reader.ReadBytes(length);

                            // 使用 MessagePack 反序列化資料
                            MessageModel message = MessagePackSerializer.Deserialize<MessageModel>(data);
                            messages.Add(message);
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


        // 假設 MessageModel 是你要反序列化的類型
        //public static List<MessageModel> ReadMessagesFromFile(string filePath)
        //{
        //    List<MessageModel> messages = new List<MessageModel>();

        //    // 讀取檔案所有資料
        //    byte[] fileData = File.ReadAllBytes(filePath);

        //    int position = 0;
        //    while (position < fileData.Length)
        //    {
        //        try
        //        {
        //            // 使用 MessagePack 反序列化資料
        //            // 每個物件的長度會隨著資料不同而不同，因此需要一個機制來確定每次要反序列化多少位元組
        //            var message = MessagePackSerializer.Deserialize<MessageModel>(fileData.AsSpan(position));
        //            messages.Add(message);

        //            // 更新 position，跳過剛剛反序列化的資料部分
        //            position += MessagePackSerializer.GetByteCount(message);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"反序列化錯誤: {ex.Message}");
        //            break;
        //        }
        //    }

        //    return messages;
        //}

    }
}