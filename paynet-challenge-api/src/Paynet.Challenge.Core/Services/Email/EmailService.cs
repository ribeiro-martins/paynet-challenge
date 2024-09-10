using System.Net;
using System.Net.Mail;
using Paynet.Challenge.Core.Settings;

namespace Paynet.Challenge.Core.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly ISettings _settings;

        public EmailService(ISettings settings)
        {
            NetworkCredential netCred = new NetworkCredential(settings.SenderEmail, settings.SenderEmailPassword);
            _smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = netCred
            };
            _settings = settings;
        }

        public void SendEmail(string to, string subject, string body, bool isBodyHtml = false)
        {
            var mailMessage = new MailMessage(_settings.SenderEmail, to, subject, body);
            mailMessage.IsBodyHtml = isBodyHtml;
            _smtpClient.Send(mailMessage);            
        }
    }
}