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

        private const string hashKey = "�A��HashKey";
        private const string hashIV = "�A��HashIV";

        public void OnPost()
        {
            MerchantID = "�A���ө��N��";

            var tradeInfo = new Dictionary<string, string>
        {
            { "MerchantID", MerchantID },
            { "RespondType", "JSON" },
            { "TimeStamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
            { "Version", "1.5" },
            { "MerchantOrderNo", "ORDER123456" },
            { "Amt", "1000" },
            { "ItemDesc", "���հӫ~" },
            { "ReturnURL", "https://�A������/Return" },
            { "NotifyURL", "https://�A������/Notify" },
            { "ClientBackURL", "https://�A������/Back" }
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
            string merchantID = "�A��MerchantID";
            string hashKey = "�A��HashKey";
            string hashIV = "�A��HashIV";

            var tradeInfo = new Dictionary<string, string>
    {
        { "MerchantID", merchantID },
        { "RespondType", "JSON" },
        { "TimeStamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
        { "Version", "1.0" },
        { "MerchantOrderNo", "ORDER20250521" },
        { "Amt", "1000" },
        { "ItemDesc", "�w���q�\�A��" },
        { "Email", "user@example.com" },
        { "LoginType", "0" },
        { "PeriodAmt", "1000" },
        { "PeriodType", "M" },          // M = ��
        { "PeriodPoint", "5" },         // �C�� 5 ������
        { "PeriodTimes", "12" },        // �`�@���� 12 ��
        { "PeriodStartType", "1" },     // �ߧY�_��
        { "ReturnURL", "https://�A������/ReturnPeriod" }
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
