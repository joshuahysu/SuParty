using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuParty.Middleware;

namespace SuParty.Pages
{
    public class ShutdownModel : PageModel
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public ShutdownModel(IHostApplicationLifetime hostApplicationLifetime)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public IActionResult OnPostStopAndShutdown()
        {
            // 停止接受新請求
            RequestControlMiddleware.CanAcceptRequests = false;

            // 延遲 30 秒後關閉應用程式
            Task.Delay(30000).ContinueWith(t =>
            {
                _hostApplicationLifetime.StopApplication();  // 停止應用程式
            });

            return RedirectToPage("Shutdown");  // 或返回其他頁面
        }
    }
}
