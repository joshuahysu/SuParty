using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
namespace SuParty.Pages.NPay
{
    public class PaymentModel : PageModel
    {
        public string MerchantID { get; set; }
        public string TradeInfo { get; set; }
        public string TradeSha { get; set; }

        private const string hashKey = "你的HashKey";
        private const string hashIV = "你的HashIV";

        public void OnPost()
        {
            MerchantID = "你的商店代號";

            var tradeInfo = new Dictionary<string, string>
        {
            { "MerchantID", MerchantID },
            { "RespondType", "JSON" },
            { "TimeStamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
            { "Version", "1.5" },
            { "MerchantOrderNo", "ORDER123456" },
            { "Amt", "1000" },
            { "ItemDesc", "測試商品" },
            { "ReturnURL", "https://你的網站/Return" },
            { "NotifyURL", "https://你的網站/Notify" },
            { "ClientBackURL", "https://你的網站/Back" }
        };

            string tradeInfoQuery = string.Join("&", tradeInfo.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            TradeInfo = EncryptAES(tradeInfoQuery, hashKey, hashIV);
            TradeSha = EncryptSHA256($"HashKey={hashKey}&{TradeInfo}&HashIV={hashIV}");
        }

        private string EncryptAES(string plainText, string key, string iv)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyBytes;
            aes.IV = ivBytes;

            using var encryptor = aes.CreateEncryptor();
            byte[] result = encryptor.TransformFinalBlock(data, 0, data.Length);
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

        private string EncryptSHA256(string value)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            byte[] hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToUpper();
        }
        public IActionResult OnPostPeriod()
        {
            string merchantID = "你的MerchantID";
            string hashKey = "你的HashKey";
            string hashIV = "你的HashIV";

            var tradeInfo = new Dictionary<string, string>
    {
        { "MerchantID", merchantID },
        { "RespondType", "JSON" },
        { "TimeStamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
        { "Version", "1.0" },
        { "MerchantOrderNo", "ORDER20250521" },
        { "Amt", "1000" },
        { "ItemDesc", "定期訂閱服務" },
        { "Email", "user@example.com" },
        { "LoginType", "0" },
        { "PeriodAmt", "1000" },
        { "PeriodType", "M" },          // M = 月
        { "PeriodPoint", "5" },         // 每月 5 號扣款
        { "PeriodTimes", "12" },        // 總共扣款 12 次
        { "PeriodStartType", "1" },     // 立即起扣
        { "ReturnURL", "https://你的網站/ReturnPeriod" }
    };

            string raw = string.Join("&", tradeInfo.Select(kv => $"{kv.Key}={kv.Value}"));
            string tradeInfoEncrypted = EncryptAES(raw, hashKey, hashIV);
            string tradeSha = EncryptSHA256($"HashKey={hashKey}&{tradeInfoEncrypted}&HashIV={hashIV}");

            ViewData["MerchantID"] = merchantID;
            ViewData["TradeInfo"] = tradeInfoEncrypted;
            ViewData["TradeSha"] = tradeSha;
            ViewData["Version"] = "1.0";

            return Page();
        }

    }
}
