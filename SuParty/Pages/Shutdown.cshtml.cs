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
            // ������s�ШD
            RequestControlMiddleware.CanAcceptRequests = false;

            // ���� 30 ����������ε{��
            Task.Delay(30000).ContinueWith(t =>
            {
                _hostApplicationLifetime.StopApplication();  // �������ε{��
            });

            return RedirectToPage("Shutdown");  // �Ϊ�^��L����
        }
    }
}
