using Huede.FMC2.Configs;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System;
using System.Linq;
using Huede.FMC2.DataClasses;
using Huede.FMC2.Entities;
using Huede.FMC2.Plugins;
using Microsoft.EntityFrameworkCore;
using Huede.FMC2.Communications;
using Huede.FMC2.Plugin;
using Newtonsoft.Json.Linq;

namespace Huede.FmcPlugin
{
    public class CreateFMCPngPlugin : IFlowPlugin
    {
        /// <summary>
        /// 通訊管理者的實體
        /// </summary>
        private static readonly CreateFMCPngPlugin _instance = new CreateFMCPngPlugin();

        /// <summary>
        /// 取得通訊管理者的實體
        /// </summary>
        public static CreateFMCPngPlugin Instance => _instance;

        /// <summary>
        /// 波形圖設定的實體
        /// </summary>
        private static readonly FMC2.Settings.Waveforms.WaveformSetting SETTING = FMC2.Settings.SettingManager.Instance.CurrentSetting.Waveform;
        IImageGenerator ImageGenerator;
        private long Id {  get; set; }

        public CreateFMCPngPlugin()
        {
            AssemblyConfig imageCofig = ConfigManager.Instance.Config.ImageGenerator;
            if (imageCofig != null)
                ImageGenerator = Utilities.Utility.Instance.CreateObject(imageCofig.Assembly, imageCofig.Type, imageCofig.Path, imageCofig.Arguments) as IImageGenerator;
            ImageGenerator.WaveformSetting = FMC2.Settings.SettingManager.Instance.CurrentSetting.Waveform;
        }

        public  ExecuteResult<JObject> Execute(ExecuteResult<JObject>[] executeResults, FlowStepConfig stepConfig)
        {
             Id = (long)PluginFlow.InputParameter.Value["TaskId"];
            string zipFilePath = CreatePngZipFile(Id);
            var data = new JObject
            {
                ["FilePath"] = zipFilePath,
            };
            return new ExecuteResult<JObject>(true, "CreateFMCPngPlugin completed", data);
        }



        /// <summary>
        /// 產出工作單上所有波形圖的png
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="zipFilePath"></param>
        public string CreatePngZipFile(long taskId)
        {
            using (Database db = new Database())
            {
                Task task = db.Task.Include(t => t.Missions).ThenInclude(m => m.Device).Include(t => t.Missions).ThenInclude(m => m.CreateUser).Include(t => t.Missions).ThenInclude(m => m.UpdateUser).Where(t => t.Id == taskId && t.State > (short)ReportState.Deleted).SingleOrDefault();

                //1.產圖的條件
                var imgNum = 1;
                var dpi = 300;

                double totalSeconds = 3600 * 8;
                DateTime startTime = task.Missions.First().StartTime;
                DateTime endTime = task.Missions.Last().EndTime ?? DateTime.Now;
                //2.產圖
                SplitFMCImage(taskId, ref imgNum, dpi, totalSeconds, startTime, endTime);
                //3.壓縮

                string folderPath = $"./{DateTime.Now.ToString("yyyyMMdd")}/{taskId}/";

                //取得資料夾內所有 PNG 檔案
                string[] pngFiles = Directory.GetFiles(folderPath, "*.png");

                //壓縮檔案放進暫存資料夾
                string zipFilePath = Path.Combine(Path.GetTempPath(), DateTime.Now.ToString("yyyyMMddHHmmss") + $"waveforms_{taskId}.zip");
                //壓縮成 ZIP
                using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                {
                    foreach (var pngFile in pngFiles)
                    {
                        var fileName = Path.GetFileName(pngFile); // 取得原始檔名
                        zipArchive.CreateEntryFromFile(pngFile, fileName);
                    }
                }
                return zipFilePath;
            }
        }

        /// <summary>
        /// 切割所選範圍內的胎心音圖片檔(可修改產圖條件變成依照mission切割)
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="imgNum">圖片編號(會累加)</param>
        /// <param name="dpi">圖片解析度</param>
        /// <param name="splitSeconds">切個的時間單位</param>
        /// <param name="startTime">開始時間</param>
        /// <param name="endTime">結束時間</param>
        private void SplitFMCImage(long id, ref int imgNum, int dpi, double splitSeconds, DateTime startTime, DateTime endTime)
        {
            string[] header = new string[1];
            header[0] = "工作單號:" + id;
            while (startTime < endTime)
            {
                // 計算當前批次的結束時間
                DateTime batchEndTime = startTime.AddSeconds(splitSeconds);
                if (batchEndTime > endTime)
                {
                    batchEndTime = endTime; // 避免超過最終結束時間
                }

                // 取得這段時間內的數據
                List<MonitorPoint> monitorPoints = FMC2.Communications.CommunicationManager.Instance
                    .GetMergedRawData(id, startTime, batchEndTime)
                    .Select(r => (MonitorPoint)r)
                    .ToList();
                var s = false;
                string filePath = "./" + DateTime.Now.ToString("yyyyMMdd") + "/" + id + "/" + startTime.ToString("yyyyMMddHHmmss") + (imgNum++) + ".png";
                if (monitorPoints.Count > 1)
                {
                    ImageGenerator.GenerateImageFile(filePath, header, dpi, monitorPoints);
                }
                else if (s)
                {
                    //裡面無資料需要生成兩個假點才能畫圖
                    monitorPoints.Add(new MonitorPoint()
                    {
                        Timestamp = startTime
                    });

                    monitorPoints.Add(new MonitorPoint()
                    {
                        Timestamp = batchEndTime
                    });
                    ImageGenerator.GenerateImageFile(filePath, header, dpi, monitorPoints);
                }

                // 更新 startTime 為下一批次的開始時間
                startTime = batchEndTime;
            }
        }

    }
}