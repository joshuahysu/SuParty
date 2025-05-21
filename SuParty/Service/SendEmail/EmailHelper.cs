
namespace SuParty.Service.SendEmail
{
    using System;
    using System.Net;
    using System.Net.Mail;

    public class EmailHelper
    {

        public static void SendEmailC(
            string smtpUser,
            string smtpPassword,
            string toEmail,
            string subject,
            string body)
        {
            EmailHelper.SendEmail(
                 smtpHost: "smtp.gmail.com",
                 smtpPort: 587,
                 smtpUser: "your_email@gmail.com",
                 smtpPassword: "your_app_password", // Gmail 建議使用 App 密碼
                 fromEmail: "your_email@gmail.com",
                 toEmail: toEmail,
                 subject: subject,
                 body: body,
                 enableSsl: true
             );
        }
        public static void SendEmail(
            string smtpHost,
            int smtpPort,
            string smtpUser,
            string smtpPassword,
            string fromEmail,
            string toEmail,
            string subject,
            string body,
            bool enableSsl = true)
        {
            try
            {
                var fromAddress = new MailAddress(fromEmail);
                var toAddress = new MailAddress(toEmail);

                using (var smtp = new SmtpClient
                {
                    Host = smtpHost,
                    Port = smtpPort,
                    EnableSsl = enableSsl,
                    Credentials = new NetworkCredential(smtpUser, smtpPassword),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 20000
                })
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false // 若要支援 HTML，改成 true
                })
                {
                    smtp.Send(message);
                }

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
                // 可選：Log 例外詳情或重新拋出
            }
        }
    }
}