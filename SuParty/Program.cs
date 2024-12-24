using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SuParty.Data;

namespace SuParty
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // 添加多國語系支援服務
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // 添加視圖和 Razor Page 本地化
            builder.Services.AddRazorPages()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();
            // 註冊 SignalR 服務
            builder.Services.AddSignalR();
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en-US", "zh-TW", "ja-JP" }; // 支援的語系
                options.SetDefaultCulture("en-US") // 預設語系
                       .AddSupportedCultures(supportedCultures)
                       .AddSupportedUICultures(supportedCultures);
            });
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddRazorPages(options =>
            {
                // 使用相對檔案路徑作為路由前綴
                options.Conventions.AddFolderRouteModelConvention("/", model =>
                {
                    foreach (var selector in model.Selectors)
                    {
                        var template = selector.AttributeRouteModel.Template;
                        selector.AttributeRouteModel.Template = "Pages/" + template;
                    }
                });
            });

            builder.Services.AddControllersWithViews()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization(); // 啟用 DataAnnotations 本地化

            var app = builder.Build();
            // 啟用本地化中介軟體
            var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage(); // 顯示詳細的錯誤頁面
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
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

   
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.MapHub<MessageHub>("/messageHub"); // 註冊 SignalR Hub 的路徑
            app.Run();
        }
    }
}
