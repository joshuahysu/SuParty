using Huede.FMC2.Plugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Huede.FMC2.Plugin
{
    internal class PluginFlow
    {
        public static AsyncLocal<JObject> InputParameter = new AsyncLocal<JObject>();

        public static List<ExecuteResult<JObject>> ExecuteFlow(List<FlowStepConfig> config)
        {
           var results = new List<ExecuteResult<JObject>>();

            foreach (var stepConfig in config.OrderBy(c => c.Step))
            {
                if (!stepConfig.Enabled)
                {
                    Console.WriteLine($"Step {stepConfig.Step} is disabled, skipping...");
                    continue;
                }

                var plugin = LoadPlugin<IFlowPlugin>(stepConfig.DllPath, stepConfig.TypeName);
                if (plugin != null)
                {
                    var previousResults = results.ToArray();
                    var result = plugin.Execute(previousResults, stepConfig);
                    results.Add(result);
                }
                else
                {
                    Console.WriteLine($"Failed to load plugin for Step {stepConfig.Step}");
                    results.Add(new ExecuteResult<JObject>(false, "Plugin load failed", null));
                }
            }


            return results;
        }

        public static List<FlowStepConfig> LoadConfig(string configPath, string flowName)
        {
            string json = File.ReadAllText(configPath);
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = true,
                        OverrideSpecifiedNames = true
                    }
                },
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var flows = JsonConvert.DeserializeObject<List<FlowConfig>>(json, settings);
            if (flows == null || flows.Count == 0)
            {
                Console.WriteLine("Invalid flow configuration or no flows found.");
                return new List<FlowStepConfig>();
            }

            var config = flows.FirstOrDefault(f => f.FlowName == flowName);
            if (config == null || config.FlowSteps == null)
            {
                Console.WriteLine($"Flow '{flowName}' not found or invalid.");
                return new List<FlowStepConfig>();
            }

            Console.WriteLine($"Loaded flow: {config.FlowName}");
            return config.FlowSteps;
        }

        /// <summary>
        /// 從指定的 DLL 路徑載入類型，若 DLL 路徑為空則從當前專案載入類型。
        /// </summary>
        /// <typeparam name="T">插件的介面或基類，載入的類型必須能轉換為此類型。</typeparam>
        /// <param name="dllPath">外部 DLL 檔案的路徑，若為空則載入當前專案類別。</param>
        /// <param name="typeName">要載入的類別完整名稱 (命名空間 + 類別名稱)。</param>
        /// <returns>回傳載入的物件實例，若發生錯誤則回傳 null。</returns>
        internal static T LoadPlugin<T>(string dllPath, string typeName) where T : class
        {
            try
            {
                Assembly assembly;

                if (string.IsNullOrEmpty(dllPath))
                {
                    // 沒有指定 DLL，從當前應用程式的 Assembly 載入
                    assembly = Assembly.GetExecutingAssembly();
                }
                else
                {
                    // 指定 DLL，載入外部 Assembly
                    string fullPath = Path.GetFullPath(dllPath);
                    assembly = Assembly.LoadFrom(fullPath);
                }

                // 取得類型資訊
                Type type = assembly.GetType(typeName);

                // 確保類型存在，且該類型可以轉換為 T
                if (type == null || !typeof(T).IsAssignableFrom(type))
                {
                    throw new Exception($"Invalid plugin type: {typeName}");
                }

                // 建立類別實例並回傳
                return (T)Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                // 當發生錯誤時，輸出錯誤訊息並回傳 null
                Console.WriteLine($"Error loading plugin: {ex.Message}");
                return null;
            }
        }
    }
    public class FlowConfig
    {
        public string FlowName { get; set; } // 流程名稱
        public List<FlowStepConfig> FlowSteps { get; set; } // 步驟列表
    }
    public interface IFlowPlugin
    {
        ExecuteResult<JObject> Execute(ExecuteResult<JObject>[] executeResults, FlowStepConfig stepConfig);
    }
    public class FlowStepConfig
    {
        public int Step { get; set; }
        public string DllPath { get; set; }
        public string TypeName { get; set; }
        public int? DependsOnStep { get; set; }
        public JObject Parameters { get; set; } // 客製化參數
        public bool Enabled { get; set; } = true; // 是否啟用，預設為 true
    }
}