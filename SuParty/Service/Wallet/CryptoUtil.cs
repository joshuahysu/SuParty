using System.Security.Cryptography;
using System.Text;

namespace SuParty.Service.Wallet
{
    public class CryptoUtil
    {
        public static string EncryptAES(string plainText, string key, string iv)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] result = encryptor.TransformFinalBlock(data, 0, data.Length);
                    return BitConverter.ToString(result).Replace("-", "").ToLower();
                }
            }
        }

        public static string EncryptSHA256(string plainText)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = Encoding.UTF8.GetBytes(plainText);
                byte[] hash = sha256.ComputeHash(data);
                return BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }
        }
    }
}
