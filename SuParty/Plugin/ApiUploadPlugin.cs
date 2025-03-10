using Huede.FMC2.Plugin;
using Huede.FMC2.Plugins;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Huede.FmcPlugin
{
    internal class ApiUploadPlugin : IFlowPlugin
    {
        /// <summary>
        /// 定義NLog實體
        /// </summary>
        private readonly Logger LOG = LogManager.GetLogger(nameof(ApiUploadPlugin));

        public ExecuteResult<JObject> Execute(ExecuteResult<JObject>[] ExecuteResult, FlowStepConfig stepConfig)
        {
            try
            { 
                DeliverConfig c = stepConfig.Parameters.ToObject<DeliverConfig>();
                if (c.IsCompresse)
                {
                    UploadAndCompress(c.FilePath, c.ZipFileName, c.Address, c.MaxRetries);
                }
                else
                {
                    if (string.IsNullOrEmpty(c.FilePath))
                    {
                        //如果使用前面步驟的產出
                        c.FilePath = stepConfig.DependsOnStep.HasValue &&                stepConfig.DependsOnStep.Value >= 0 &&stepConfig.DependsOnStep.Value < ExecuteResult.Length+1? ExecuteResult[stepConfig.DependsOnStep.Value-1].Data?["FilePath"]?.ToString() ?? string.Empty : string.Empty;

                        if(c.FilePath=="")c.FilePath = PluginFlow.InputParameter.Value["FileToUploadPath"].ToString();
                    }                  
                    
                    UploadFileWithExponentialBackoff(c.FilePath, c.Address, c.MaxRetries);
                }
            }
            catch (Exception ex)
            {
                return new ExecuteResult<JObject>(false, ex.ToString(), null);            
            }

            return new ExecuteResult<JObject>(true, null, null);
        }


        /// <summary>
        /// 壓縮且上傳 
        /// </summary>
        /// <param name="sourceFile">原始檔位置</param>
        /// <param name="zipFile">壓縮檔檔名</param>
        /// <param name="apiUrl"></param>
        public async Task UploadAndCompress(string sourceFile,string zipFile, string apiUrl, int maxRetries = 5)
        {
            await CompressFileAsync(sourceFile, zipFile);
            await UploadFileWithExponentialBackoff(zipFile, apiUrl, maxRetries);
        }

        /// <summary>
        /// 使用Api上傳檔案且包含重傳功能
        /// </summary>
        /// <param name="filePath">上傳檔案的檔案路徑</param>
        /// <param name="apiUrl">API 位址</param>
        /// <param name="maxRetries">最大重傳次數</param>
        /// <returns></returns>
        public async Task UploadFileWithExponentialBackoff(string filePath, string apiUrl, int maxRetries = 5)
        {
            int attempt = 0;
            while (attempt < maxRetries)
            {
                try
                {
                    attempt++;
                    await UploadFile(filePath, apiUrl);
                    return; // 成功時返回
                }
                catch (Exception ex)
                {
                    Log(nameof(UploadFileWithExponentialBackoff), $"重試 {attempt} 次失敗: {ex.Message}");
    
                    if (attempt >= maxRetries)
                    {
                        Log(nameof(UploadFileWithExponentialBackoff), "達到最大重試次數，請稍後再試");
                        throw;  // 如果已達最大重試次數，則丟出異常
                    }

                    // 指數退避，重試間隔為 2^attempt 秒
                    int delay = (int)Math.Pow(2, attempt) * 1000;
                    await Task.Delay(delay); // 等待指定的時間再重試
                }
            }
        }

        /// <summary>
        /// 使用Api上傳檔案
        /// </summary>
        /// <param name="filePath">上傳檔案的檔案路徑</param>
        /// <param name="apiUrl">API 位址</param>
        public async Task UploadFile(string filePath, string apiUrl)
        {
            using (var httpClient = new HttpClient())
            using (var form = new MultipartFormDataContent())
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var content = new StreamContent(fileStream))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                form.Add(content, "file", Path.GetFileName(filePath));

                var response = await httpClient.PostAsync(apiUrl, form);
                string result = await response.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// 壓縮檔案(zip)
        /// </summary>
        /// <param name="sourceFile">來源檔案</param>
        /// <param name="zipFile">輸出的 ZIP 檔案</param>
        /// <returns></returns>
        public async Task CompressFileAsync(string sourceFile, string zipFile)
        {
            byte[] buffer = new byte[4096]; // 設定 buffer 大小
            using (var zipStream = new FileStream(zipFile, FileMode.Create))
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                var entry = archive.CreateEntry(Path.GetFileName(sourceFile), CompressionLevel.Optimal);
                using (var entryStream = entry.Open())
                using (var fileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                {
                    int bytesRead;
                    while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await entryStream.WriteAsync(buffer, 0, bytesRead);
                    }
                }
            }
        }


        #region Log

        /// <summary>
        /// 寫入上傳相關日誌
        /// </summary>
        /// <param name="action">動作</param>
        /// <param name="log">日誌內容</param>
        private void Log(string action, string detail)
         {
            Thread t = Thread.CurrentThread;
            LOG.Error($"[{t.Name ?? t.ManagedThreadId.ToString()}] {nameof(ApiUploadPlugin)} {action.ToUpper()} {detail}");
         }

        #endregion
    }


    public class DeliverConfig
    {

        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 壓縮檔案名稱
        /// </summary>
        public string ZipFileName { get; set; }

        /// <summary>
        /// 路徑
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 重試次數
        /// </summary>
        public int MaxRetries { get; set; }

        /// <summary>
        /// 是否用壓縮
        /// </summary>
        public bool IsCompresse { get; set; }
    }
}