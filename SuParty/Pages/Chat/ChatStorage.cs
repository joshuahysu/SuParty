using System.Globalization;
using System.Runtime.CompilerServices;
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
        public static void SaveMessage(string chatroomId, MessageModel message)
        {
            // 確保基礎目錄存在
            Directory.CreateDirectory(BasePath);

            // 聊天室目錄
            string chatroomPath = Path.Combine(BasePath, chatroomId);
            Directory.CreateDirectory(chatroomPath);

            // 檔案名稱以日期命名
            string fileName = $"{message.CreatedAt:yyyy-MM-dd}.txt";
            string filePath = Path.Combine(chatroomPath, fileName);

            AppendMessageToFile(filePath, message);
        }
        // 使用 AggressiveInlining 提示編譯器內嵌此函式
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AppendMessageToFile(string filePath, MessageModel message)
        {
            // 將物件序列化為 UTF-8 位元組
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(message);

            using (var stream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            using (var bufferedStream = new BufferedStream(stream))
            {
                bufferedStream.Write(jsonBytes, 0, jsonBytes.Length);
                bufferedStream.WriteByte(0x0A);
            }

        }

        /// <summary>
        /// 軟刪除
        /// </summary>
        /// <param name="chatroomId"></param>
        public static void DeleteChatroom(string chatroomId)
        {         // 聊天室目錄
            string chatroomPath = Path.Combine(BasePath, chatroomId);
            FolderRename(chatroomPath, chatroomPath+"_Bak");
        }

        /// <summary>
        /// 資料夾改名及搬檔案
        /// </summary>
        /// <param name="oldFolderPath"></param>
        /// <param name="newFolderPath"></param>
        public static void FolderRename(string oldFolderPath, string newFolderPath) {
            try
            {
                if (Directory.Exists(oldFolderPath))
                {
                    // 檢查新資料夾名稱是否已存在
                    if (Directory.Exists(newFolderPath))
                    {
                        // 新資料夾已存在，將舊資料夾內的檔案搬到新資料夾
                        Console.WriteLine($"資料夾 {newFolderPath} 已存在，開始搬移檔案。");

                        foreach (string filePath in Directory.GetFiles(oldFolderPath))
                        {
                            string fileName = Path.GetFileName(filePath);
                            string destinationFilePath = Path.Combine(newFolderPath, fileName);

                            // 如果檔案名稱衝突，加上亂數
                            if (File.Exists(destinationFilePath))
                            {
                                string fileExtension = Path.GetExtension(fileName);
                                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                                string uniqueFileName = $"{fileNameWithoutExtension}_{Guid.NewGuid():N}{fileExtension}";
                                destinationFilePath = Path.Combine(newFolderPath, uniqueFileName);
                            }

                            File.Move(filePath, destinationFilePath);
                            Console.WriteLine($"檔案 {fileName} 已移動至 {destinationFilePath}");
                        }

                        // 搬移完成後刪除舊資料夾
                        Directory.Delete(oldFolderPath);
                        Console.WriteLine($"資料夾 {oldFolderPath} 已刪除。");
                    }
                    else
                    {
                        // 使用 Directory.Move 重新命名資料夾
                        Directory.Move(oldFolderPath, newFolderPath);
                        Console.WriteLine($"資料夾已重新命名為 {newFolderPath}");
                    }
                }
                else
                {
                    Console.WriteLine($"資料夾 {oldFolderPath} 不存在。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"重新命名資料夾時發生錯誤: {ex.Message}");
            }

        }

        /// <summary>
        /// 讀取指定聊天室和日期的訊息
        /// </summary>
        /// <param name="chatroomId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<MessageModel> ReadMessages(string chatroomId, DateTime? date=null)
        {
            if(date==null)
                date=DateTime.Now;
            string chatroomPath = Path.Combine("messages", chatroomId);
            string fileName = $"{date:yyyy-MM-dd}.txt";
            string filePath = Path.Combine(chatroomPath, fileName);
            //未來優化效能直接傳檔
            var messages = new List<MessageModel>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    // 將每行 JSON 反序列化為 Message 物件
                    var message = JsonSerializer.Deserialize<MessageModel>(line);
                    if (message != null)
                    {
                        messages.Add(message);
                    }
                }
            }

            return messages;
        }
        /// <summary>
        /// 讀取指定聊天室的訊息
        /// </summary>
        /// <param name="chatroomId"></param>
        /// <returns></returns>
        public static List<MessageModel> ReadAndMergeFilesDefalut(string chatroomId)
        {
            string folderPath = Path.Combine("messages", chatroomId);// 資料夾路徑

            // 列出資料夾內所有符合 yyyy-MM-dd.txt 格式的檔案
            var validFiles = Directory.GetFiles(folderPath, "*.txt")
                                      .Select(Path.GetFileNameWithoutExtension)
                                      .Where(fileName => DateTime.TryParseExact(fileName, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                                      .OrderBy(fileName => fileName) // 按日期升序排列
                                      .ToList();

            if (!validFiles.Any())
            {
                return new List<MessageModel>();
            }

            // 開始處理檔案
            var messages = ReadAndMergeFiles(folderPath, validFiles);
            return messages;
        }
        public static List<MessageModel> ReadAndMergeFiles(string folderPath, List<string> validFiles=null)
        {

            List<MessageModel> messages = new List<MessageModel>();

            foreach (var fileName in validFiles)
            {
                string filePath = Path.Combine(folderPath, $"{fileName}.txt");

                if (File.Exists(filePath))
                {
                    // 讀取檔案並將每行 JSON 反序列化為 MessageModel
                    foreach (var line in File.ReadLines(filePath))
                    {
                        try
                        {
                            var message = JsonSerializer.Deserialize<MessageModel>(line);
                            if (message != null)
                            {
                                messages.Add(message);
                            }
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"反序列化錯誤: {ex.Message} (檔案: {filePath})");
                        }
                    }

                    Console.WriteLine($"已讀取檔案: {fileName}.txt, 當前資料筆數: {messages.Count}");
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
    }
}