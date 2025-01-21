using Google.Api;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SuParty.Data;
using SuParty.Middleware;
using TronNet;

namespace SuParty
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 設定本地化資源的路徑
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // 註冊 Razor Page 並啟用視圖本地化及資料註解本地化
            builder.Services.AddRazorPages()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            // 註冊 SignalR 支援
            builder.Services.AddSignalR();
            //builder.Services.AddSignalR().AddAzureSignalR();雲端
            // 設定支援的語言及預設語言
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en-US", "zh-TW", "ja-JP" }; // 支援的語言
                options.SetDefaultCulture("en-US") // 設定預設語言
                       .AddSupportedCultures(supportedCultures)
                       .AddSupportedUICultures(supportedCultures);
            });

            // 設定MSSQL資料庫連線字串
            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            //builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(connectionString)); // 使用 SQL Server 作為資料庫

            // 使用 SQLite 資料庫
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=application.db"));  // SQLite 的資料庫檔案名稱


            // 開發期間顯示資料庫相關錯誤訊息
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // 設定認證
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = IdentityConstants.ApplicationScheme;
            //    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            //})
            //.AddGoogle(options =>
            //{
            //    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            //    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            //})
            //.AddFacebook(options =>
            //{
            //    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
            //    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
            //});

            // 設定 Identity 認證，並啟用帳號確認
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // 設定 Razor Pages 相關路由
            builder.Services.AddRazorPages(options =>
            {
                // 配置 Razor Pages 的路由，使其包含 "Pages/" 前綴
                options.Conventions.AddFolderRouteModelConvention("/", model =>
                {
                    foreach (var selector in model.Selectors)
                    {
                        var template = selector.AttributeRouteModel.Template;
                        selector.AttributeRouteModel.Template = "Pages/" + template;
                    }
                });
            });

            // 註冊 MVC 控制器並啟用視圖本地化及資料註解本地化
            builder.Services.AddControllersWithViews()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization(); // 启用 DataAnnotations 本地化

            //USDT
            //builder.Services.AddTronNet(x =>
            //{
            //    x.Network = TronNetwork.MainNet;
            //    x.Channel = new GrpcChannelOption { Host = "grpc.shasta.trongrid.io", Port = 50051 };
            //    x.SolidityChannel = new GrpcChannelOption { Host = "grpc.shasta.trongrid.io", Port = 50052 };
            //    x.ApiKey = "input your api key";
            //});

            var app = builder.Build();

            // 配置請求管線中的本地化選項
            var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);

            // 配置 HTTP 請求處理管線
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint(); // 執行資料庫遷移
                app.UseDeveloperExceptionPage(); // 顯示開發環境中的錯誤頁面
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // 生產環境中的錯誤頁面
                app.UseHsts(); // 啟用 HTTP 嚴格傳輸安全性
            }

            // 處理來自 URL 的 culture 查詢參數來更改當前文化
            app.Use(async (context, next) =>
            {
                var cultureQuery = context.Request.Query["culture"];
                if (!string.IsNullOrEmpty(cultureQuery))
                {
                    var culture = new System.Globalization.CultureInfo(cultureQuery);
                    System.Globalization.CultureInfo.CurrentCulture = culture;
                    System.Globalization.CultureInfo.CurrentUICulture = culture;
                }
                await next();
            });

            app.UseHttpsRedirection(); // 重定向 HTTP 到 HTTPS
            app.UseStaticFiles(); // 啟用靜態檔案服務

            app.UseRouting(); // 啟用路由功能

            // 註冊身份驗證與授權中介軟體
            app.UseAuthentication();
            app.UseAuthorization();

            // 註冊 RequestControlMiddleware
            app.UseMiddleware<RequestControlMiddleware>();
            // 設定預設路由
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages(); // 設定 Razor Pages 路由
            app.MapHub<MessageHub>("/messageHub"); // 設定 SignalR Hub 路由

            app.Run(); // 啟動應用程式
        }
    }
}