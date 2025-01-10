using System.Text;

namespace SuParty.Service.LinePay
{
    public class LinePay
    {
        private static readonly string linePayUrl = "https://api-pay.line.me/v2/payments/request"; // API URL
        private static readonly string channelId = "YOUR_CHANNEL_ID"; // 商戶 ID
        private static readonly string channelSecret = "YOUR_CHANNEL_SECRET"; // API 金鑰
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task Pay()
        {
            var paymentRequest = new
            {
                amount = 1000, // 支付金額
                currency = "TWD", // 貨幣
                orderId = Guid.NewGuid().ToString(), // 訂單編號
                productName = "商品名稱",
                confirmUrl = "https://yourdomain.com/confirm", // 確認支付的回呼 URL
                cancelUrl = "https://yourdomain.com/cancel" // 取消支付的回呼 URL
            };

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            client.DefaultRequestHeaders.Add("X-LINE-ChannelSecret", channelSecret);

            var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(paymentRequest), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(linePayUrl, jsonContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Response: " + responseContent);
        }
        /// <summary>
        /// 確認支付
        /// 支付請求後，Line Pay 會根據 confirmUrl 進行回應，您可以在這個 URL 中處理確認支付的操作。例如，您需要檢查是否支付成功並更新訂單狀態。
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public static async Task ConfirmPayment(string transactionId)
        {
            var confirmUrl = "https://api-pay.line.me/v2/payments/{0}/confirm";  // 確認支付 API
            var client = new HttpClient();

            var paymentConfirmRequest = new
            {
                amount = 1000,
                currency = "TWD"
            };

            var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(paymentConfirmRequest), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(string.Format(confirmUrl, transactionId), jsonContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Payment Confirm Response: " + responseContent);
        }

    }
}
