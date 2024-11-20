using System.Net.Mail;
using System.Net;

namespace Vfurniture.Areas.Admin.Reponsitory
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("thanhvu1462003@gmail.com", "udaymodfqeexatpp")
            };

            return client.SendMailAsync(
                new MailMessage(from: "thanhvu1462003@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
