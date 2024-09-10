namespace Paynet.Challenge.Core.Services.Email
{
    public interface IEmailService
    {
        public void SendEmail(string to, string subject, string body, bool isBodyHtml = false);
    }
}