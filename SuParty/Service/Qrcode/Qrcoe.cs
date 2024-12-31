using QRCoder;
using System;
using System.Buffers.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace SuParty.Service.Qrcode
{
    public class Qrcode1
    {
        public static string CreateQrcode(string url) {
            // 要生成的內容
            //string url = "https://www.example.com";

            // 初始化 QR Code 生成器

            using QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            using QRCodeData data = qRCodeGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using PngByteQRCode qRCode = new PngByteQRCode(data);
            byte[] image = qRCode.GetGraphic(10,
                              darkColorRgba: new byte[] { 0, 90, 0 }, // RGB
                              lightColorRgba: new byte[] { 255, 255, 255 }); // RGB
            string base64Image = Convert.ToBase64String(image);
            Console.WriteLine($"QR Code Base64: {base64Image}");
            return base64Image;
            //            < !--將 Base64 編碼嵌入圖片 src 屬性中 -->
            //< img src = "data:image/png;base64,{{Base64QRCode}}" alt = "QR Code" />

        }
    }
}
