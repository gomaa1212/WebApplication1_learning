using System.Net;
using System.Net.Mail;
using WebApplication1.Models.Services.Interfaces;

namespace WebApplication1.Models.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smptClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential()
                {
                    UserName = "abdoelrahmanmohammed060@gmail.com",
                    Password = "gzry tkkj qmac tlni"
                },
                EnableSsl = true
            };

            var mailMessage = new MailMessage()
            {
                From = new MailAddress("abdoelrahmanmohammed060@gmail.com"),
                Body = body,
                Subject = subject,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            await smptClient.SendMailAsync(mailMessage);

        }
        public string GenerateCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
