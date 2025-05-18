using Newtonsoft.Json;
using System;
using System.IO;
namespace SuParty.Unit
{
    public static class FileUtil
    {
        /// <summary>
        /// 安全地將任意資料寫入檔案，先寫入暫存檔，再原子替換原始檔案，可選備份。
        /// </summary>
        /// <param name="filePath">目標檔案路徑</param>
        /// <param name="writeAction">寫入暫存檔案的動作（接收 Stream）</param>
        /// <param name="backupOldFile">是否備份原始檔案（.bak）</param>
        public static void SafeReplaceFile(string filePath, Action<Stream> writeAction, bool backupOldFile = false)
        {
            string tempPath = filePath + ".tmp";
            string backupPath = filePath + ".bak";

            try
            {
                // 1. 寫入暫存檔案
                using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    writeAction(fs);
                }

                // 2. 備份舊檔（如需要）
                if (backupOldFile && File.Exists(filePath))
                {
                    File.Copy(filePath, backupPath, overwrite: true);
                }

                // 3. 原子性替換檔案
                File.Move(tempPath, filePath, overwrite: true);
            }
            catch (Exception ex)
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
                throw new IOException("SafeReplaceFile failed: " + ex.Message, ex);
            }
        }
        /// <summary>
        /// 安全寫入文字內容（跨平台）
        /// </summary>
        public static void SafeWriteTextFile(string filePath, string content, bool backupOldFile = false)
        {
            SafeReplaceFile(filePath, stream =>
            {
                using var writer = new StreamWriter(stream);
                writer.Write(content);
            }, backupOldFile);
        }

        /// <summary>
        /// 安全寫入二進位資料（跨平台）
        /// </summary>
        public static void SafeWriteBinaryFile(string filePath, byte[] data, bool backupOldFile = false)
        {
            SafeReplaceFile(filePath, stream =>
            {
                stream.Write(data, 0, data.Length);
            }, backupOldFile);
        }
        /// <summary>
        /// 安全寫入Json內容（跨平台）
        /// </summary>
        public static void SafeWriteJsonFile<T>(string filePath, T data, bool backupOldFile = false)
        {
            FileUtil.SafeReplaceFile(filePath, stream =>
            {
                using var writer = new StreamWriter(stream);
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                writer.Write(json);
            }, backupOldFile);
        }

    }

}
