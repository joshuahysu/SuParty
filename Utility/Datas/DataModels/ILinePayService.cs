
namespace SuParty.Data.DataModel
{
    public interface ILinePayService
    {
        /// <summary>
        /// 呼叫 LINE Pay Request API 產生付款交易，回傳付款頁面網址
        /// </summary>
        /// <param name="dto">付款請求資料</param>
        /// <returns>付款頁面網址</returns>
        Task<string> RequestPaymentAsync(PaymentRequestDto dto);

        /// <summary>
        /// 確認付款 API (optional)
        /// </summary>
        Task<bool> ConfirmPaymentAsync(string transactionId, string orderId);
    }

}