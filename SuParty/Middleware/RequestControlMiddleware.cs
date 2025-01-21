namespace SuParty.Middleware
{
    public class RequestControlMiddleware
    {
        private readonly RequestDelegate _next;
        public static volatile bool CanAcceptRequests=true;

        public RequestControlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 如果應用程式正在關閉，阻止新的請求
            if (!CanAcceptRequests)
            {
                context.Response.StatusCode = 503;  // 服務不可用
                await context.Response.WriteAsync("服務即將關閉，請稍後再試。");
                return;
            }

            // 如果可以接受請求，繼續處理
            await _next(context);
        }
    }

}
