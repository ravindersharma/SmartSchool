using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using SmartSchool.Application.Common.Interfaces;

namespace SmartSchool.Infrastructure.Services.Email
{
    public class SendGridEmailSender : IEmailSender
    {

        private readonly IConfiguration _config;
        public SendGridEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var key = _config["SendGrid:ApiKey"];
            if (key == null) throw new Exception("SendGrid API Key not configured");

            var cleint = new SendGridClient(key);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_config["SendGrid:FromEmail"] ?? "admin@smartschool.com", "SmartSchool"),
                Subject = subject,
                HtmlContent = body
            };

            msg.AddTo(new EmailAddress(to));

            await cleint.SendEmailAsync(msg);
        }
    }
}
